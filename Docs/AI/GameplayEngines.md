# New Master — Gameplay Engine Expansion

## Implemented in this stage

### Raid Engine 2.0
- Shield consumption remains deterministic and one-use per blocked raid.
- Target defense now reduces loot by a bounded mitigation percentage.
- Successful raids expose awarded loot, defense mitigation, calculated village damage and counter-attack bounty.
- RaidGameService.TryRaidDetailed preserves the existing RaidResult API while exposing the richer result for UI and future online authority.
- NewMasterRaidPanel consumes the detailed result and persists a counter-attack opportunity in GameState.CounterAttack.

### World Boss Engine
- WorldBossState is persisted inside GameState.
- Boss identity is tied to the current world's bossId.
- Boss health, personal contribution, defeat state and reward claim are normalized.
- Attacks consume one energy and apply bounded damage.
- Defeated bosses can be claimed exactly once.
- Runtime fallback worlds already assign a boss every tenth world, so the system works without generated ScriptableObject content.

### Guild / Co-op foundation
- GuildCoopService adds bounded cooperative score contributions.
- Milestone crossings are reported deterministically.
- Milestone rewards scale safely without integer overflow.
- Guild contribution leaderboards provide deterministic ranking with stable player-ID tie breaking.
- Leaderboard snapshots are bounded and presentation-ready without pretending to be a server-authoritative online ranking.
- The service remains in the Social assembly so the Core assembly does not acquire a dependency cycle.

## Persistence

No save version bump was required. WorldBossState and CounterAttackState are additive fields and are initialized by GameStateMigrations.Normalize.

## Validation boundary

Repository validation is automated by GitHub Actions. Unity Editor compilation, Play Mode, visual UI inspection, Android device execution and production store integration remain environment-dependent and must not be inferred from repository validation alone.
