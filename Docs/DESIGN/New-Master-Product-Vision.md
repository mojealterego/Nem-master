# New Master — Product Vision & Successor Blueprint

## 1. Product identity

**New Master** is an original next-generation mobile game designed as a successor to the Coin Master-style progression formula.

The design goal is not to reproduce the existing game. The goal is to retain the immediately understandable reward loop while expanding the player experience into a deeper world, village, raid, collection, social and live-content ecosystem.

## 2. Core player fantasy

The player is building a persistent personal empire while constantly deciding how to spend scarce opportunities:

**Spin → Win → Build → Defend → Raid → Collect → Evolve → Unlock → Repeat**

Every layer must feed another layer so that progression is systemic rather than a sequence of disconnected menus.

## 3. Differentiation pillars

### A. Worlds as systems

Worlds are not only visual skins. Each world can define:

- symbol pool;
- reward curve;
- energy behavior;
- village rules;
- raid targets;
- bosses;
- events;
- collection sets;
- presentation language.

The architecture must allow hundreds of worlds without duplicating gameplay code.

### B. Village as a living place

The village is persistent progression, not merely a level screen.

Planned systems:

- buildings;
- upgrade tiers;
- defensive structures;
- visual evolution;
- repair/destruction states;
- customization;
- milestone rewards.

### C. Raids with consequences

Raids should create meaningful interaction between attack, defense and recovery.

Planned systems:

- attack selection;
- shields;
- loot;
- counter-attacks;
- target protection;
- event modifiers;
- server-authoritative resolution.

### D. Collections

Collections create long-term goals independent of raw currency.

Planned systems:

- cards;
- albums;
- pets;
- artifacts;
- set bonuses;
- duplicate handling;
- limited-time collections.

### E. Social progression

Social systems should create cooperative reasons to return.

Planned systems:

- friends;
- guilds;
- cooperative events;
- leaderboards;
- gifts;
- social missions.

### F. Live world

The game is designed to support changing content without replacing the client every time.

Planned systems:

- seasons;
- daily events;
- missions;
- challenges;
- rotating worlds;
- remote configuration;
- analytics-driven balancing.

## 4. Technical direction

The runtime is organized around explicit domain ownership:

- **Core** — state, spin rules and orchestration.
- **Worlds** — data-driven world definitions and catalogs.
- **Progression** — unlock and advancement rules.
- **Village** — building definitions and upgrade state.
- **Future domains** — raids, collections, social, LiveOps, networking, persistence and presentation.

Deterministic gameplay rules remain separated from Unity presentation code wherever practical.

## 5. Online authority

The production architecture is intended to move sensitive economy decisions to server authority.

The client should be treated as a presentation and input surface, not as the final authority for:

- currency grants;
- purchases;
- raid results;
- collection grants;
- leaderboard values;
- event rewards.

## 6. Performance direction

Android is the first target.

Performance constraints are architectural:

- data-driven content instead of duplicated scenes;
- Addressables for scalable content delivery;
- controlled allocations;
- bounded runtime collections;
- pooling where measurement justifies it;
- scalable VFX;
- adaptive presentation quality;
- explicit loading boundaries.

## 7. Production phases

### Phase I — Core vertical slice
Spin, rewards, energy, world progression, village upgrades and production HUD.

### Phase II — World & village
World presentation, building visuals, progression feedback, save/migration and content pipeline.

### Phase III — Raids & collections
Raid resolution, defenses, collections, pets and long-term progression.

### Phase IV — Online
Authentication, cloud save, server-authoritative economy, anti-cheat and matchmaking.

### Phase V — Social & LiveOps
Guilds, friends, events, seasons, missions, leaderboards and remote configuration.

### Phase VI — Production presentation
Character pipeline, animation, VFX, audio, haptics, cinematic events and final Android optimization.

## 8. Definition of success

New Master should feel immediately readable in its first minute and substantially deeper after sustained play.

The successor strategy is therefore based on **breadth of systems, depth of progression and originality of execution**, not on reproducing another game's assets, UI or exact mechanics.
