# New Master

**New Master** is an original next-generation mobile game designed as a successor to the Coin Master-style progression formula — not a 1:1 clone.

The goal is to take the accessible spin → reward → economy → progression loop and evolve it into a substantially broader game platform with original systems, economy, worlds, villages, raids, collections, social mechanics, LiveOps and cinematic presentation.

## Product direction

New Master is being built around these principles:

- **Original IP and systems** rather than a mechanical copy.
- **Data-driven content** so worlds and progression can scale without bespoke gameplay code.
- **Strong moment-to-moment feedback** through animation, VFX, audio and haptics.
- **Long-term progression** through worlds, villages, collections and events.
- **Online-first foundations** for cloud persistence, server-authoritative economy and anti-cheat.
- **Live game architecture** for seasons, events, missions and rotating content.
- **Android-first performance** with scalable presentation and content delivery.

## Current implemented foundation

- Typed player/game state.
- Energy-gated spin loop.
- Deterministic, testable spin outcome rules.
- Data-driven world definitions and world catalog.
- World unlock progression.
- Village building definitions and upgrade progression.
- Runtime assembly boundary.
- EditMode gameplay tests.
- New Master bootstrap entry point.

## Target architecture

### CORE ENGINE
Economy · RNG/Spin · Energy · Progression · Inventory · Save/Migration

### WORLD ENGINE
Large-scale world catalog · World rules · Bosses · Seasonal content

### VILLAGE ENGINE
Buildings · Upgrades · Defenses · Customization · Destruction/repair

### RAID ENGINE
Raids · Attacks · Shields · Loot · Counter-attacks

### COLLECTION ENGINE
Cards · Albums · Pets · Artifacts · Collections

### SOCIAL ENGINE
Friends · Guilds · Chat · Leaderboards · Cooperative events

### LIVE OPS
Daily events · Seasons · Missions · Challenges · Dynamic events

### ONLINE
Authentication · Cloud Save · Server authority · Anti-cheat · Matchmaking

### MONETIZATION
Shop · Bundles · Battle Pass · Rewarded Ads · Google Play Billing

### PRESENTATION
Characters · Animation · VFX · Shaders · Audio · Haptics · Cinematic events

## Roadmap

1. Production boot scene and HUD
2. Village runtime presentation
3. Save/migration layer
4. Server-authoritative economy
5. Authentication and cloud sync
6. Addressables and remote content
7. Shop and Google Play Billing
8. Raid/attack system
9. Collections and pets
10. Social/guilds/leaderboards
11. LiveOps/event framework
12. Character, animation, VFX and audio pipeline
13. Analytics and anti-cheat
14. Android profiling and release validation

## Repository

- Unity project target: **Unity 6.3**
- Primary platform: **Android**
- Repository slug: `Nem-master`
- Product name: **New Master**

> The repository is an active development foundation. Unity Editor compilation, Play Mode, device testing and Android player-build validation must still be executed before release readiness can be claimed.
