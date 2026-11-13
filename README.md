# mikedopp.github.io

Personal site for [mikedopp.github.io](https://mikedopp.github.io): a portfolio for an infrastructure automation engineer, drawn as a server rack. Each public project is a mounted unit with a status LED, skills sit on a patch panel, and contact links are outlets on the PDU.

Built with Jekyll and served by GitHub Pages from `master` at the repo root. No theme gem, no CDN: layouts, CSS, JS and fonts all live in this repo.

## Where things live

| What | File |
| --- | --- |
| Home page (the rack) | `index.html` |
| Featured projects and skills | `_data/rack.yml` |
| "Also racked" links (editor-managed) | `_data/projects.yml` |
| About / spec sheet | `aboutme.md` |
| Site settings, email, profile links | `_config.yml` |
| Layouts | `_layouts/base.html`, `page.html`, `post.html` |
| Nav switch, contact PDU, title block | `_includes/` |
| Styles | `assets/css/site.css` |
| Rack behaviour (U snapping, rail numbers, service views) | `assets/js/rack.js` |
| Fonts (Archivo, Martian Mono; SIL OFL 1.1, the only third-party files) | `assets/fonts/` |
| Product and design notes | `PRODUCT.md`, `DESIGN.md`, `.impeccable/` |

### Adding a project to the rack

Add an entry under `units:` in `_data/rack.yml` with `name`, `finish` (`silver` or `graphite`), `state` (`released` or `prerelease`), `version`, `status`, `pitch`, `shots`, `specs`, `note` and `links`. Put screenshots in `assets/img/work/` as WebP. Only public repositories belong on the rack.

## Preview locally

Needs Ruby 3.3 with DevKit.

```powershell
bundle install
bundle exec jekyll serve
```

Open `http://127.0.0.1:4000`.

## Offline editor

`tools/MikeSiteEditor` is a Windows app for writing posts, editing root pages, managing `_data/projects.yml`, inserting media and committing. Start it with `.\Start-SiteEditor.ps1` or `Start-SiteEditor.cmd`. Posts it saves show up on the rack as a change log.

```powershell
dotnet build .\tools\MikeSiteEditor\MikeSiteEditor.csproj
dotnet run --project .\tools\MikeSiteEditor.Tests\MikeSiteEditor.Tests.csproj
```

## Dependencies

| Dependency | Where |
| --- | --- |
| Ruby 3.3 + Bundler, Jekyll, jekyll-sitemap, kramdown-parser-gfm | `Gemfile` (local preview only; GitHub Pages builds on its own) |
| .NET SDK | `tools/MikeSiteEditor` (editor only) |
| Git + GitHub credentials | publishing and the editor's Commit + Push |
| GitHub Pages | hosting, repo `mikedopp/mikedopp.github.io`, branch `master`, root |

## Publish

Push to `master`. GitHub Pages builds the site from the repository root. Pages only serves this repo while it is public (or on a paid plan).

## License

Copyright (c) 2026 Mike Dopp (mikedopp). All rights reserved. The site, its code, design and content are proprietary; no license is granted to copy, modify or redistribute any of it. The bundled fonts are under the SIL Open Font License 1.1. See [LICENSE](LICENSE).
