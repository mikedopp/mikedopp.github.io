---
name: mikedopp.github.io
description: Portfolio drawn as a front-view rack elevation; every project is a mounted unit with an honest status LED.
colors:
  sheet: "#E4E8EA"
  sheet-ink: "#262B30"
  sheet-ink-2: "#4A535B"
  sheet-line: "#9DA6AE"
  steel-950: "#121417"
  steel-900: "#1A1D21"
  steel-850: "#202428"
  steel-800: "#2A2F34"
  steel-700: "#3A4148"
  steel-500: "#69727A"
  steel-400: "#8A939B"
  steel-300: "#AEB6BC"
  steel-200: "#C9CFD3"
  steel-100: "#DEE2E5"
  steel-50: "#EEF1F3"
  ink: "#121417"
  ink-2: "#39414A"
  ink-inv: "#EEF1F3"
  ink-inv-2: "#B8C0C6"
  tape: "#F2D024"
  tape-deep: "#D9B70C"
  tape-rim: "#8F7A06"
  tape-ink: "#121417"
  led-green: "#33C25A"
  led-amber: "#FF9F1A"
  led-off: "#474E55"
  jack-black: "#050607"
  screen-white: "#F2F5F6"
  paper: "#F8FAFA"
typography:
  display:
    fontFamily: "Archivo, Arial Narrow, system-ui, sans-serif"
    fontSize: "clamp(2.35rem, 6.6vw, 5.25rem)"
    fontWeight: 800
    lineHeight: 0.92
    letterSpacing: "0.005em"
    fontVariation: "\"wdth\" 125"
  headline:
    fontFamily: "Archivo, Arial Narrow, system-ui, sans-serif"
    fontSize: "clamp(2rem, 5vw, 3.75rem)"
    fontWeight: 800
    lineHeight: 0.92
    letterSpacing: "0.005em"
    fontVariation: "\"wdth\" 125"
  title:
    fontFamily: "Archivo, Arial Narrow, system-ui, sans-serif"
    fontSize: "clamp(1.35rem, 2.6vw, 2rem)"
    fontWeight: 800
    lineHeight: 0.92
    letterSpacing: "0.005em"
    fontVariation: "\"wdth\" 125"
  title-sm:
    fontFamily: "Archivo, Arial Narrow, system-ui, sans-serif"
    fontSize: "clamp(1.15rem, 2vw, 1.5rem)"
    fontWeight: 800
    lineHeight: 0.92
    fontVariation: "\"wdth\" 125"
  lede:
    fontFamily: "Archivo, Arial Narrow, system-ui, sans-serif"
    fontSize: "clamp(1.2rem, 2vw, 1.5rem)"
    fontWeight: 450
    lineHeight: 1.35
  body:
    fontFamily: "Archivo, Arial Narrow, system-ui, sans-serif"
    fontSize: "1.0625rem"
    fontWeight: 400
    lineHeight: 1.55
    fontFeature: "\"tnum\" 1"
  label:
    fontFamily: "Archivo, Arial Narrow, system-ui, sans-serif"
    fontSize: "0.8125rem"
    fontWeight: 750
    lineHeight: 1.1
    letterSpacing: "0.07em"
    fontVariation: "\"wdth\" 75"
  data:
    fontFamily: "Martian Mono, ui-monospace, Consolas, monospace"
    fontSize: "0.6875rem"
    fontWeight: 400
    letterSpacing: "0.04em"
    fontVariation: "\"wdth\" 87.5"
rounded:
  tape: "2px"
  sm: "3px"
  md: "4px"
  btn: "6px"
  handle: "10px"
  led: "50%"
spacing:
  u: "48px"
  rail: "60px"
  pad-x: "clamp(30px, 4.2vw, 52px)"
  xs: "6px"
  sm: "12px"
  md: "18px"
  lg: "22px"
components:
  button-primary:
    backgroundColor: "{colors.tape}"
    textColor: "{colors.tape-ink}"
    rounded: "{rounded.btn}"
    padding: "0 22px"
    height: "50px"
    typography: "{typography.label}"
  button-primary-hover:
    backgroundColor: "#F7DA3C"
  button-steel:
    backgroundColor: "{colors.steel-800}"
    textColor: "{colors.ink-inv}"
    rounded: "{rounded.btn}"
    padding: "0 22px"
    height: "50px"
    typography: "{typography.label}"
  button-steel-hover:
    backgroundColor: "{colors.steel-700}"
  tape:
    backgroundColor: "{colors.tape}"
    textColor: "{colors.tape-ink}"
    rounded: "{rounded.tape}"
    padding: "0.5em 0.75em 0.45em"
    typography: "{typography.label}"
  tape-outline:
    textColor: "{colors.ink-inv}"
    rounded: "{rounded.tape}"
    padding: "0.5em 0.75em 0.45em"
    typography: "{typography.label}"
  port:
    textColor: "{colors.ink-inv}"
    rounded: "{rounded.sm}"
    padding: "0 10px 0 6px"
    height: "40px"
  handle:
    textColor: "{colors.ink-inv}"
    rounded: "{rounded.handle}"
    padding: "0 14px 0 10px"
    height: "44px"
  spec-sticker:
    backgroundColor: "{colors.steel-50}"
    textColor: "{colors.ink}"
    rounded: "{rounded.sm}"
    padding: "10px 12px"
  kvm-screen:
    backgroundColor: "{colors.screen-white}"
    textColor: "{colors.ink}"
    rounded: "{rounded.btn}"
    padding: "clamp(24px, 3.4vw, 40px)"
  doc-sheet:
    backgroundColor: "{colors.paper}"
    textColor: "{colors.ink}"
    rounded: "{rounded.sm}"
    padding: "clamp(22px, 3.4vw, 40px)"
---

# Design System: mikedopp.github.io

## Overview

**Creative North Star: "The Rack Elevation"**

The site is a technical drawing of one server rack seen from the front. A cool light-gray drafting sheet, framed by a hairline border and a heavy outer rule, carries a dimension line across the top and a title block at the bottom right. On the sheet stands a single graphite rack with cage-nut rails and U numbers; every piece of content is a unit bolted into it. Navigation is a 1U top-of-rack switch whose ports are the links, the introduction is a 4U appliance with a silver faceplate, projects are 3U servers that slide out on rails into a service view, skills are a patch panel, the about text sits on a KVM console drawer, contact is a PDU, and empty space is a blanking panel.

The world is dense, physical and literal. Everything is rendered as an object with an edge, a screw, a seam or a label, and nothing floats free of the rack. The palette is restricted to the inks such a rack actually has: steel in many tones, label-maker yellow tape, and LED states. Type does its work through Archivo's width axis: wide uppercase nameplates for model names and condensed caps for tape labels, with Martian Mono reserved for machine data.

State is shown as a mark with words beside it, never as colour alone. A released unit carries solid tape and a steady green LED; an unproven one carries hairline-outline tape and a blinking amber LED, and its text says what has not been proven.

**Key Characteristics:**
- One rack, stacked units with no gaps between them; vertical rhythm in 48px rack units.
- Restricted inks: sheet gray, steel, tape yellow, LED green and amber.
- Archivo's width axis carries hierarchy (wide nameplates, condensed labels).
- Martian Mono only for data: U numbers, asset IDs, LED codes, dates, ratings.
- Physical depth: inset bevels on faceplates, small cast shadows on objects that sit proud (tape, stickers, buttons, tags).
- One signature motion: a server unit slides out on its rails; LEDs blink. Both stop under reduced motion.

## Colors

A cold, mostly achromatic steel world with exactly one warm accent and two signal lights.

### Primary
- **Label-Maker Yellow** (tape): Every label in the rack: section headings on cable managers, the role tape under the hero name, status tapes on server units, patch-panel port labels, the primary EMAIL button, the live PDU outlet, the skip link and text selection. Its gloss gradient and cast shadow make it read as tape stuck on metal.
- **Tape Shadow Yellow** (tape-deep): The lower stop of yellow gradients (patched-cable boots, the live outlet socket).
- **Tape Rim Olive** (tape-rim): The 1px rim on the primary button and the inset ring of the live outlet; it gives yellow an edge against light silver.
- **Tape Ink** (tape-ink): Text on yellow; same value as Rack Black.

### Secondary
- **Link LED Green** (led-green): Steady "on" and "released" state, the power LED, the activity LED, lit switch ports and patched-port LEDs. Always paired with a glow and adjacent words.
- **Fault Amber** (led-amber): "In progress, not yet proven." Blinks on a 2.4s stepped cycle. Never used for decoration.
- **Dark LED** (led-off): An LED that is off (fault LED with nothing logged, idle port LEDs, empty patch ports).

### Neutral
- **Drafting Sheet** (sheet): The page background outside the rack, also the title-block cells and the dimension-label knockout.
- **Sheet Ink** (sheet-ink) / **Sheet Ink Secondary** (sheet-ink-2) / **Sheet Rule** (sheet-line): Drawing text, dimension lines and the sheet border.
- **Steel ramp** (steel-950 to steel-50): The rack and its parts. steel-950 is the bay behind units and the tray floor; steel-900 the rack frame; steel-850 the rails; steel-800 default faceplates and steel buttons; steel-700 rail edges and button hover; steel-400 hairline dividers, rail numbers and port numbers on dark; steel-300 and steel-200 table and fact rules on light; steel-100 inline code; steel-50 the top of spec stickers.
- **Rack Black** (ink) / **Ink Secondary** (ink-2): Text on silver faceplates, paper, stickers and tags.
- **Inverse Ink** (ink-inv) / **Inverse Ink Secondary** (ink-inv-2): Text on graphite and black faceplates and in the service tray.
- **Jack Black** (jack-black): The void inside jacks, sockets and screenshot bezels.
- **Screen White** (screen-white): The KVM console screen surface.
- **Paper** (paper): The document sheet for pages and posts.

Faceplate finishes are gradients built from these neutrals and are applied as a class, not picked freely: silver (#E3E7EA to #C3CACF with fine vertical brushing, dark ink), graphite (#31373D to #272C31, inverse ink) and black (#1E2125 to #17191C, inverse ink).

### Named Rules
**The Restricted Inks Rule.** Steel, tape yellow and the two LED colours are the whole palette. No blue links, no brand gradients, no second accent.

**The State Is a Mark Rule.** An LED or tape style never carries state alone. The state is always written next to it ("v0.9.0 · Prerelease · Offline-proven", "Accepting interviews"), and an unproven state uses outline tape plus amber, a released one solid tape plus green.

**The Yellow Is a Label Rule.** Yellow appears only where a real rack would have tape or a coloured part (labels, cable boots, the live outlet, the one primary button). It is never a background wash or a decorative stripe.

## Typography

**Display Font:** Archivo variable (wdth 62–125, wght 100–900), with Arial Narrow and system-ui fallback
**Body Font:** Archivo, same file
**Label/Mono Font:** Martian Mono variable, with ui-monospace and Consolas fallback

**Character:** One family stretched across its width axis does the work of three: expanded heavy caps read as bezel nameplates, condensed bold caps read as label-maker tape, and normal-width text carries reading. Martian Mono is the machine voice. Both fonts are self-hosted (SIL OFL 1.1).

### Hierarchy
- **Display** (800, wdth 125, clamp(2.35rem, 6.6vw, 5.25rem), lh 0.92, uppercase): The hero model name only.
- **Headline** (800, wdth 125, clamp(2rem, 5vw, 3.75rem), lh 0.92, uppercase): Page and post titles, the 404 code.
- **Title** (800, wdth 125, clamp(1.35rem, 2.6vw, 2rem), uppercase): Server unit names and the About heading; a small step (clamp(1.15rem, 2vw, 1.5rem)) for the PDU heading.
- **Lede** (450, clamp(1.2rem, 2vw, 1.5rem), lh 1.35, max 34ch): The one-sentence pitch on the hero.
- **Body** (400, 1.0625rem, lh 1.55, tabular numerals): Pitches (max 52ch), the KVM text (lh 1.6, max 68ch) and document prose (lh 1.65, max 72ch).
- **Label** (750, wdth 75, 0.8125rem, 0.07em, uppercase): Tape. Related condensed caps sit between wdth 70 and 90 at 0.75–0.9375rem: buttons (wdth 90, 0.06em), ports (wdth 80, 0.08em), patch labels (wdth 70), spec strip (wdth 75, 650), tray and table headers (wdth 80, 0.1–0.12em).
- **Data** (Martian Mono, 0.5625–0.6875rem, wdth 87.5, 0.04em): U numbers, the dimension line, asset ID, LED codes, dates, port numbers, the vent plate and the PDU rating.

### Named Rules
**The Width Axis Rule.** Hierarchy comes from width before size: wide (125) for things that are named, condensed (70–90) for things that are labelled. Never set a nameplate condensed or a tape label wide.

**The Mono Is Data Rule.** Martian Mono is for identifiers, numbers and codes that a machine would print. Sentences are never set in mono.

## Layout

The page is a drawing sheet (max 1240px, 16px from the viewport, 56px top padding for the dimension line) bordered by a 1px rule and a 3px outer rule offset 6px. Inside it the rack is a three-column grid: a 60px rail, the bay, a 60px rail. The rails repeat a 48px tile (one U) with three cage-nut holes and a boundary tick, and number the U's down the side.

Units stack in the bay edge to edge, separated only by a 1px near-black seam. Unit heights are multiples of the U where the part is a real rack part: 1U for the switch, cable managers, link units and blanking panels; 3U minimum for server faceplates; the appliance ends in a 1U vent strip; the KVM keyboard tray is 2U. Horizontal padding inside units is one token (clamp(30px, 4.2vw, 52px)). Internal gaps step through 6, 12, 18 and 22px.

The hero appliance is a two-column grid (content, then a 220–260px side column for the asset tag and LED block). Server faceplates are a three-column grid (identity, pitch, actions) that collapses to two at 1080px and one at 760px. The patch panel runs 8 ports per row, 6 at 1080px, 3 at 760px. Documents use a 72ch sheet beside a sticky 200–250px contact column.

At 760px and below the rails narrow to 26px and lose their numbers, the switch stacks its brand above a 4-up port grid, the hero stacks main, side and vent, the split role tape prints as two lines, PDU outlets become a single column and the title block goes to two columns. At 380px the switch model code hides.

### Named Rules
**The U Rule.** Vertical rhythm is the 48px rack unit. Content is added as another unit in the stack, not as a free-floating card with margins around it.

## Elevation & Depth

Depth is physical and shallow. The rack itself casts one soft shadow onto the sheet. Faceplates are not lifted; they read through inset bevels (a light top edge and a dark bottom edge) and four corner screws drawn as radial gradients (two, vertically centred, on 1U parts). Objects that are stuck on or plugged in sit proud with small cast shadows. Recesses (jacks, sockets, the outlet strip, the LED block, the cable-manager brush) use inset shadows.

### Shadow Vocabulary
- **Rack on sheet** (`box-shadow: 0 2px 0 #121417, 0 18px 40px -16px rgba(18,20,23,0.55)`): The rack only.
- **Silver bevel** (`box-shadow: inset 0 1px 0 #F5F7F8, inset 0 -2px 0 rgba(0,0,0,0.12)`): Silver faceplates; graphite and black use the same shape at lower white alpha.
- **Tape** (`box-shadow: 0 1px 1px rgba(0,0,0,0.3), 0 3px 8px -2px rgba(0,0,0,0.25)`): Label tape.
- **Sticker** (`box-shadow: 0 1px 1px rgba(0,0,0,0.5), 0 4px 10px -6px rgba(0,0,0,0.6)`): Spec stickers in the service tray.
- **Button** (`box-shadow: 0 2px 5px -1px rgba(18,20,23,0.35)`; hover `0 6px 12px -4px rgba(18,20,23,0.4)` with a 1px lift; active `0 1px 2px rgba(18,20,23,0.3)` pressed 1px down): Front-panel buttons.
- **Pulled faceplate** (`box-shadow: 0 14px 22px -12px rgba(0,0,0,0.7)`, 2px lift): A server unit whose service view is open.
- **Recess** (`box-shadow: inset 0 2px 6px rgba(0,0,0,0.6)`): The PDU outlet strip; jacks and sockets use a 2px inset ring.
- **LED glow** (`0 0 7px 1px` at 0.55 alpha of the LED colour): Lit LEDs only.

### Named Rules
**The Proud Object Rule.** A cast shadow means the thing is physically on top of the faceplate (tape, sticker, tag, button, plug) or has been pulled out. Faceplates themselves only get bevels.

## Shapes

Corners are machined, not soft. Tape and plates are 2px, ports, stickers and code are 3px, the rack top, screenshots and LED block 4px, buttons and the KVM screen 6px. The only larger radius is the 10px pull handle. LEDs are circles. Slight rotation is reserved for things that were stuck on by hand: the hero tape (-0.6deg) and the asset tag (1.2deg). Screenshots sit in a 6px near-black bezel. Lines are hairlines (1px) except the sheet's 3px outer rule and the title block's 1.5px frame. Focus is a 3px outline offset 3px with a 3px radius: Rack Black on light metal and paper, tape yellow on dark steel.

## Components

### Buttons
Front-panel push buttons, solid and tactile.
- **Shape:** gently squared (6px), 50px tall, 22px side padding (18px on mobile), 1px near-black rim.
- **Primary:** tape yellow with tape ink and an olive rim; carries a green LED before the label. One per surface.
- **Steel:** steel-800 with inverse ink; hover steel-700.
- **Hover / Active:** 1px lift with a deeper shadow on hover; 1px press with a tight shadow on active; 140ms ease-out; transitions off under reduced motion.

### Tape (signature)
Label-maker tape: condensed bold caps on yellow with a gloss highlight and a small cast shadow, 2px corners. Variants: large (role tape, slight rotation), split (role and domain; two lines at 760px and below, separator hidden), outline (1.5px yellow inset ring, transparent fill, faceplate ink) for unproven states and lab-only skills, and small inline (the "Note" label in the tray).

### LEDs
9px circles with a highlight. Green steady, amber blinking on a stepped 2.4s cycle, activity flicker on 1.7s, off as a dark dot. Always aria-hidden with the state in adjacent text. Under forced colours they keep their own colours.

### Navigation (top-of-rack switch)
A 1U black switch: wide brand name plus mono model code on the left, four RJ45 ports on the right. Each port is a drawn jack with a 4px link LED that lights green on hover and on the active section; port labels are condensed caps. On mobile the ports become a 4-up grid with the jack stacked over the label.

### Server unit with service view
A 3U faceplate (silver or black finish) carrying the unit name as a title nameplate, a status tape with its LED, a pitch, a source link and a pull handle (2px steel-400 inset ring, grip lines, chevron). Opening the handle lifts the faceplate 2px and slides a tray out beneath it: a jack-black floor with telescoping rails drawn on both sides, screenshots in bezels, spec stickers and links. The tray opens with a grid-row transition (520ms) plus a 24px slide and fade, on the rail easing curve cubic-bezier(0.22, 1, 0.36, 1); reduced motion snaps it open or shut. Without JavaScript the service view is open.

### Patch panel
Yellow port labels over drawn jacks with two-digit mono numbers. Every port is patched: a yellow cable boot and a green link LED. The panel lists production skills only; nothing in-progress or "fundamentals" is shown.

### Cards / Containers
There are no generic cards. Containers are rack parts: faceplates (finish classes), the KVM screen (screen white in a 12px black bezel over a keyboard tray), the document paper sheet (paper, 3px, soft shadow), spec stickers, the foil asset tag (diagonal foil sheen, grayscale portrait, barcode) and PDU outlets (drawn C13 sockets; the live one is yellow).

### Title block
A drafting title block at the bottom right of the sheet: a 3-column grid of cells divided by 1px sheet-ink lines, condensed caps field names over values, a full-width drawing title in wide caps. Two columns on mobile.

## Do's and Don'ts

### Do:
- **Do** add new content as a rack unit with a finish class (silver, graphite or black), stacked edge to edge with the 1px seam.
- **Do** label sections with tape on a cable manager, and write every status in words next to its LED or tape style.
- **Do** use outline tape plus amber for anything not yet proven, and solid tape plus green only for released, proven work.
- **Do** set names in wide Archivo caps (wdth 125, 800) and labels in condensed Archivo caps (wdth 70–90, 650–750).
- **Do** keep Martian Mono to identifiers, numbers and codes.
- **Do** keep motion to the rail slide and LED behaviour, and make both steady under reduced motion.
- **Do** use the Rack Black focus ring on light surfaces and the tape-yellow ring on dark steel.

### Don't:
- **Don't** introduce a hue outside steel, tape yellow and the two LED colours.
- **Don't** let an LED colour or tape style be the only carrier of a state.
- **Don't** float content in rounded cards with gaps between them; the rack is continuous.
- **Don't** put cast shadows on faceplates at rest; shadows belong to objects sitting on them or a pulled unit.
- **Don't** use yellow as a background fill for a whole unit or as decoration.
- **Don't** set sentences in Martian Mono or nameplates in condensed widths.
- **Don't** round corners beyond 6px, except the 10px pull handle and circular LEDs.
