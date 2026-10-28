"""Merge per-category issue files into issues.json and a Markdown catalog.

Usage: python merge_issues.py <notes_dir> --json <out.json> --md <out.md> [--curation <curation.json>]

The optional curation file holds maintainer edits that survive a research refresh:
  {"drop": ["issue-id", "issue-id/fix-id"],
   "checkIds": {"issue-id": ["diagnose-check-id"]},
   "actions": {"issue-id/fix-id": "script:name | open:ms-settings:page"}}

Entries that break the schema or the evidence bar are rejected with a reason.
Exit code 1 if anything was rejected, so the coordinator notices.
"""
import argparse
import datetime
import json
import re
import sys
from pathlib import Path
from urllib.parse import urlparse

CATEGORIES = {
    "bluetooth": "Bluetooth",
    "no-output": "No sound / no output device",
    "default-device": "Default device and switching",
    "driver-realtek": "Drivers (Realtek, Intel SST, Conexant)",
    "hdmi-dp-usb": "HDMI, DisplayPort and USB audio",
    "quality": "Crackling, latency and sound quality",
    "microphone": "Microphone",
    "services-update": "Audio services and Windows Update",
}
SOURCE_TYPES = {"microsoft", "stackexchange", "reddit", "forum", "vendor"}
RISKS = {"low", "medium", "high"}
CONFIDENCE = {"verified", "strong"}
OS = {"win10", "win11"}
ID_RE = re.compile(r"^[a-z0-9]+(-[a-z0-9]+)*$")
# Same rules as FixAction.Parse in the app: an embedded script or a settings page.
ACTION_RE = re.compile(r"^(script:[a-z0-9-]{1,64}|open:(ms-settings:[A-Za-z0-9-]+|mmsys\.cpl))$")
# Commands that fetch or launch something from outside Windows are never allowed.
BANNED_COMMAND = re.compile(
    r"invoke-webrequest|invoke-restmethod|\biwr\b|\birm\b|curl|wget|start-bitstransfer|downloadstring|downloadfile|iex\b|invoke-expression|https?://",
    re.IGNORECASE,
)


def host(url):
    h = urlparse(url).hostname or ""
    return h[4:] if h.startswith("www.") else h


def check_fix(fix):
    errs = []
    if not ID_RE.match(str(fix.get("id", ""))):
        errs.append("bad fix id")
    if not fix.get("title"):
        errs.append("missing title")
    if not fix.get("steps"):
        errs.append("no steps")
    cmd = fix.get("command")
    if cmd is not None and (not isinstance(cmd, str) or BANNED_COMMAND.search(cmd)):
        errs.append("command downloads or invokes remote content")
    if fix.get("action") is not None and not ACTION_RE.match(str(fix["action"])):
        errs.append(f"bad action {fix['action']!r}")
    for key in ("requiresAdmin", "reversible", "requiresRestorePoint"):
        if not isinstance(fix.get(key), bool):
            errs.append(f"{key} must be true/false")
    if fix.get("risk") not in RISKS:
        errs.append("bad risk")
    if fix.get("risk") == "high" and fix.get("requiresRestorePoint") is not True:
        errs.append("high risk fix must require a restore point")
    if fix.get("confidence") not in CONFIDENCE:
        errs.append("bad confidence")
    sources = fix.get("sources") or []
    hosts = set()
    for s in sources:
        url = s.get("url", "")
        if urlparse(url).scheme != "https" or not host(url):
            errs.append(f"bad source url {url!r}")
        if s.get("type") not in SOURCE_TYPES:
            errs.append(f"bad source type {s.get('type')!r}")
        if not s.get("signal"):
            errs.append(f"source {url} has no confirmation signal")
        hosts.add(host(url))
    if len(sources) < 2 or len(hosts) < 2:
        errs.append("needs 2 sources from 2 different sites")
    return errs


def check_issue(issue):
    errs = []
    if not ID_RE.match(str(issue.get("id", ""))):
        errs.append("bad id")
    if issue.get("category") not in CATEGORIES:
        errs.append(f"bad category {issue.get('category')!r}")
    for key in ("title",):
        if not issue.get(key):
            errs.append(f"missing {key}")
    for key in ("symptoms", "causes"):
        if not issue.get(key):
            errs.append(f"no {key}")
    if not issue.get("os") or not set(issue["os"]) <= OS:
        errs.append("bad os list")
    if not (issue.get("detection") or {}).get("signals"):
        errs.append("no detection signals")
    issue.setdefault("detection", {}).setdefault("checkIds", [])
    good_fixes = []
    seen = set()
    for fix in issue.get("fixes") or []:
        fix_errs = check_fix(fix)
        if fix.get("id") in seen:
            fix_errs.append("duplicate fix id")
        if fix_errs:
            errs.append(f"fix {fix.get('id')!r} dropped: " + "; ".join(fix_errs))
        else:
            seen.add(fix["id"])
            good_fixes.append(fix)
    issue["fixes"] = good_fixes
    fatal = [e for e in errs if not e.startswith("fix ")]
    if not good_fixes:
        fatal.append("no valid fixes left")
    return fatal, [e for e in errs if e.startswith("fix ")]


def render_md(issues, generated):
    out = [
        "# Windows sound issues and verified fixes",
        "",
        f"Generated {generated}. Every fix has at least two independent sources. "
        "`verified` means an official Microsoft or vendor source, or an accepted high-vote answer, backs it; "
        "`strong` means independent community confirmation.",
        "",
        "Try fixes in the order listed: they go from least to most invasive.",
        "",
    ]
    for cat, label in CATEGORIES.items():
        group = [i for i in issues if i["category"] == cat]
        if not group:
            continue
        out += [f"## {label}", ""]
        for issue in group:
            out += [f"### {issue['title']}", "", f"`{issue['id']}` · {', '.join(issue['os'])}", ""]
            out += ["**Symptoms**", ""] + [f"- {s}" for s in issue["symptoms"]] + [""]
            out += ["**Causes**", ""] + [f"- {c}" for c in issue["causes"]] + [""]
            for n, fix in enumerate(issue["fixes"], 1):
                flags = [fix["confidence"], f"risk: {fix['risk']}"]
                if fix["requiresAdmin"]:
                    flags.append("admin")
                if fix["requiresRestorePoint"]:
                    flags.append("create a restore point first")
                out += [f"**Fix {n}: {fix['title']}** ({', '.join(flags)})", ""]
                out += [f"{k}. {s}" for k, s in enumerate(fix["steps"], 1)] + [""]
                if fix.get("command"):
                    out += ["```powershell", fix["command"], "```", ""]
                out += ["Sources: " + " · ".join(f"[{host(s['url'])}]({s['url']}) ({s['signal']})" for s in fix["sources"]), ""]
    return "\n".join(out)


def main():
    ap = argparse.ArgumentParser()
    ap.add_argument("notes_dir")
    ap.add_argument("--json", required=True)
    ap.add_argument("--md", required=True)
    ap.add_argument("--curation", help="maintainer edits applied on top of the research notes")
    args = ap.parse_args()

    curation = json.loads(Path(args.curation).read_text(encoding="utf-8")) if args.curation else {}
    drop = set(curation.get("drop", []))
    check_ids = curation.get("checkIds", {})
    actions = curation.get("actions", {})
    used = set()

    merged, rejected = {}, []
    for path in sorted(Path(args.notes_dir).glob("*.json")):
        try:
            entries = json.loads(path.read_text(encoding="utf-8"))
        except json.JSONDecodeError as e:
            rejected.append(f"{path.name}: invalid JSON ({e})")
            continue
        for issue in entries:
            iid = issue.get("id")
            if iid in drop:
                used.add(iid)
                continue
            fixes = []
            for fix in issue.get("fixes") or []:
                key = f"{iid}/{fix.get('id')}"
                if key in drop:
                    used.add(key)
                    continue
                if key in actions:
                    used.add(key)
                    fix["action"] = actions[key]
                fixes.append(fix)
            issue["fixes"] = fixes
            if iid in check_ids:
                used.add(iid)
                issue.setdefault("detection", {})["checkIds"] = check_ids[iid]
            fatal, dropped = check_issue(issue)
            rejected += [f"{path.name}:{issue.get('id')}: {d}" for d in dropped]
            if fatal:
                rejected.append(f"{path.name}:{issue.get('id')}: rejected: " + "; ".join(fatal))
            elif issue["id"] in merged:
                # Same issue found by two researchers: keep the first, add new fixes.
                known = {f["id"] for f in merged[issue["id"]]["fixes"]}
                merged[issue["id"]]["fixes"] += [f for f in issue["fixes"] if f["id"] not in known]
            else:
                merged[issue["id"]] = issue

    # A curation entry that matched nothing means the research changed under it.
    for key in (drop | set(check_ids) | set(actions)) - used:
        rejected.append(f"curation: {key!r} matches no issue or fix")

    generated = datetime.date.today().isoformat()
    issues = sorted(merged.values(), key=lambda i: (list(CATEGORIES).index(i["category"]), i["id"]))
    Path(args.json).parent.mkdir(parents=True, exist_ok=True)
    Path(args.md).parent.mkdir(parents=True, exist_ok=True)
    Path(args.json).write_text(
        json.dumps({"schemaVersion": 1, "generated": generated, "issues": issues}, indent=2, ensure_ascii=False) + "\n",
        encoding="utf-8",
    )
    Path(args.md).write_text(render_md(issues, generated), encoding="utf-8")

    fixes = sum(len(i["fixes"]) for i in issues)
    print(f"{len(issues)} issues, {fixes} fixes written.")
    for r in rejected:
        print("REJECTED", r)
    return 1 if rejected else 0


if __name__ == "__main__":
    sys.exit(main())
