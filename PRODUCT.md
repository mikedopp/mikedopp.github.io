# Product

<!-- impeccable:product-schema 1 -->

## Platform

web

## Stack

Jekyll on GitHub Pages (existing). Pages builds from `master` at the repo root. The offline editor in `tools/MikeSiteEditor` writes posts with `layout: post`, root pages with front matter, `_data/projects.yml`, and uploads under `assets/uploads`, so those layouts and paths must keep working. Delegated: replace the Beautiful Jekyll theme's look with the site's own layouts and CSS; no client framework.

## Users

Recruiters, hiring managers and interviewers checking out Mike Dopp before or after reading a resume or LinkedIn profile. They have a minute or two, often on a phone, and they want to know: what does he do, is he any good, can I see proof, how do I reach him.

## Product Purpose

Get Mike hired. The site is the public, permanent URL behind the resume: a portfolio for an infrastructure automation engineer. Success is a visitor who leaves knowing what he does, having seen real work, and with a one-click way to contact him.

## Positioning

He ships tooling that is honest about what it has and hasn't proven: plans with hashes, receipts for every step, "optimism is not evidence." The proof is public, working repos with releases and screenshots, not a skills word cloud.

## Operating Context

Visitors arrive from a resume, LinkedIn, or the GitHub profile. They may click through to GitHub repos and releases. They are not technical in every case: a recruiter reads the summary, an engineering manager opens the repos.

## Capabilities and Constraints

- Static site, no backend, no tracking required.
- Voice: professional with a dry, sarcastic edge (user's words: "professional sarcastic but need it to get me a job"). The jokes never undercut competence; the facts stay straight.
- Only public work is linked. Private repos are not named or linked unless the user approves each one.
- No employer names, and no AI tool names, anywhere in tracked content.
- Show only skills he claims outright. Never show learning-stage items, "fundamentals"/"basics" qualifiers, a current-focus or "now learning" list, or anything that admits a gap (user, 2026-10-09: "Dont admit what I dont know"). Items the README qualifies (Terraform, Kubernetes, Linux, CI/CD) are left off entirely, never upgraded.
- Repo is currently private, so Pages is off and the site 404s. Making it public needs the user's explicit go-ahead.
- Contact email: `mikedopp@mikedopp.com` (confirmed by the user 2026-10-09).
- License: proprietary, all rights reserved to Mike Dopp (LICENSE); only the OFL fonts are third-party.

## Brand Commitments

- Name: Mike Dopp. Handle: `mikedopp` on GitHub, LinkedIn, YouTube, Instagram.
- Title used publicly: Automation Specialist, Infrastructure Automation (PowerShell, Bash, Python).
- Location: Utah.
- Portrait: `assets/img/mike-dopp.webp` (shipped; shown grayscale on the asset tag), from the original `assets/img/TransparentMEdownload.png` (transparent cut-out, kept in the repo, excluded from the build).

## Evidence on Hand

From the public `mikedopp/Profile` README (user-published):
- 15+ years across Windows, VMware, on-prem systems, Azure/AWS.
- Four peer-nominated awards in three months for automation work.
- Active in VMUG Salt Lake City; volunteer IT support for local community services.
- Skills: PowerShell, Bash, Python; VMware vSphere, vCenter, PowerCLI; Windows Server, unattended installs, IIS, PSWindowsUpdate, Chocolatey; Azure, AWS, ARM/Bicep; YAML, JSON, Git; Windows containers; C#, .NET/WPF.
- Also in that README's skills table: GitHub Pages, scheduled jobs, configuration as code; Operations: troubleshooting, documentation, incident response, end-user support.

Public projects with READMEs, releases and real screenshots:
- Multi_Install_ISO (github.com/mikedopp/Multi_Install_ISO): builds vSphere and Hyper-V servers straight from install ISOs; plan with SHA-256, answer-media ISOs, receipts per step. 0.9.0 prerelease; offline-proven only, no VM built yet. Screenshots in `docs/screenshots/`.
- BoardScout (github.com/mikedopp/BoardScout): portable Windows motherboard, bandwidth, driver, storage and OS audit app; .NET 10; MIT; v1.9.0 released 2026-10-09. Screenshots in `docs/screenshots/`. 14 tagged releases since 2026-08-22; every release from v1.1.0 on ships a SHA-256 checksum file.

Absent, never fabricate: testimonials, employer names, client logos, metrics beyond those above, certifications, years per skill.

## Product Principles

1. Proof before adjectives: every claim points at something a visitor can open.
2. The joke is in the delivery, never in the facts.
3. Recruiter in 10 seconds, engineer in 2 minutes: summary first, depth on demand.
4. Say what isn't proven yet. Honesty is the brand.

## Accessibility & Inclusion

WCAG 2.2 AA contrast and keyboard access; respects reduced motion and reduced transparency; readable on a phone.
