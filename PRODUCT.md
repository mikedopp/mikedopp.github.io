# Product

<!-- impeccable:product-schema 1 -->

## Platform

web

## Stack

Jekyll on GitHub Pages (existing). Pages builds from `master` at the repo root. The offline editor in `tools/MikeSiteEditor` writes posts with `layout: post`, root pages with front matter, `_data/projects.yml`, and uploads under `assets/uploads`, so those layouts and paths must keep working. Delegated: replace the Beautiful Jekyll theme's look with the site's own layouts and CSS; no client framework.

## Users

Recruiters, hiring managers and interviewers checking out Mike Dopp before or after reading a resume or LinkedIn profile. They have a minute or two, often on a phone, and they want to know: what does he do, is he any good, can I see proof, how do I reach him.

## Product Purpose

Get Mike hired. The site is the public, permanent URL behind the resume: a portfolio for a DevOps and SRE engineer who automates everything that ships or breaks (deployments, releases, certificates, failovers, hardware, software) and puts AI to work with guardrails. Positioning set by the user on 2026-10-10: "more DevOps SRE, AI Automation" than infrastructure. Success is a visitor who leaves knowing what he does, having seen real work, and with a one-click way to contact him.

## Positioning

He automates the whole delivery path, from release pipelines to certificate lifecycles to failover, and fences AI in with templates and the same checks as any other code. The proof is concrete numbers and public, working repos with releases and screenshots, not a skills word cloud.

## Operating Context

Visitors arrive from a resume, LinkedIn, or the GitHub profile. They may click through to GitHub repos and releases. They are not technical in every case: a recruiter reads the summary, an engineering manager opens the repos.

## Capabilities and Constraints

- Static site, no backend, no tracking required.
- Voice: professional with a dry, sarcastic edge (user's words: "professional sarcastic but need it to get me a job"). The jokes never undercut competence; the facts stay straight.
- Only public work is linked. Private repos are not named or linked unless the user approves each one.
- No employer names, and no AI assistant or vendor brand names, anywhere in tracked content. Model runtimes and techniques (Ollama, RAG, embeddings) are fine as skills.
- Show only skills he claims outright. Never show learning-stage items, "fundamentals"/"basics" qualifiers, a current-focus or "now learning" list, or anything that admits a gap (user, 2026-10-09: "Dont admit what I dont know"). Items the README qualifies (Terraform, Kubernetes, Linux, CI/CD) are left off entirely, never upgraded.
- The repo is public and GitHub Pages serves `master` from the repo root at https://mikedopp.github.io (live since 2026-10-10). Every push to `master` deploys.
- Contact email: `mikedopp@mikedopp.com` (confirmed by the user 2026-10-09).
- License: proprietary, all rights reserved to Mike Dopp (LICENSE); only the OFL fonts are third-party.

## Brand Commitments

- Name: Mike Dopp. Handle: `mikedopp` on GitHub, LinkedIn, YouTube, Instagram.
- Title used publicly: DevOps & SRE engineer · Release pipelines · AI automation (set by the user 2026-10-10; replaces "Automation Specialist").
- Experience: 20+ years in enterprise infrastructure, 10+ in DevOps and automation (the user's own resumes; supersedes the older "15+" in the Profile README).
- Location: Utah.
- Portrait: `assets/img/mike-dopp.webp` (shipped; shown grayscale on the asset tag), from the original `assets/img/TransparentMEdownload.png` (transparent cut-out, kept in the repo, excluded from the build).

## Evidence on Hand

From the public `mikedopp/Profile` README (user-published):
- 15+ years across Windows, VMware, on-prem systems, Azure/AWS.
- Four peer-nominated awards in three months for automation work.
- Active in VMUG Salt Lake City; volunteer IT support for local community services.
- Skills: PowerShell, Bash, Python; VMware vSphere, vCenter, PowerCLI; Windows Server, unattended installs, IIS, PSWindowsUpdate, Chocolatey; Azure, AWS, ARM/Bicep; YAML, JSON, Git; Windows containers; C#, .NET/WPF.
- Also in that README's skills table: GitHub Pages, scheduled jobs, configuration as code; Operations: troubleshooting, documentation, incident response, end-user support.

From the user's own resumes (local files, not in this repo; shown without employer or internal tool names):
- Azure DevOps Classic and YAML pipelines, release definitions and REST integrations; multi-region release pipelines into AWS; Octopus Deploy release workflows; TeamCity build automation.
- Pipeline-gated failover across two datacenters, cut from 35+ minutes to under 10.
- Certificate lifecycle automated across 20+ servers: vault retrieval, binding to IIS/SSRS/RabbitMQ, verification, expiry alerting.
- 4,000+ reusable PowerShell functions; AI-written code gated by parser, security, lint, test, build and smoke checks.
- Led a small DevOps team through an on-prem to AWS move; infrastructure costs down 30%.
- Config-driven server and app-stack builds; setup time down 50%.
- Independent project: local RAG search over 28,914 chunks, 21.8 s to 30–50 ms per query.
- Skills also listed there: Docker, AKS, Key Vault, Front Door, App Services, Entra ID, AWS EC2/S3/IAM, PKI/TLS, SQL Server Always On, Azure Monitor, Datadog, Dynatrace, Serilog/SEQ, UI and browser automation, RAG/embeddings, prompt engineering, Ollama.
- Do not publish: employer names, internal tool names, or current-employer topology details (counts of availability groups, specific load-balancer and messaging products in their estate).

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
