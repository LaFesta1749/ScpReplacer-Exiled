# ScpReplacer

[![Downloads](https://img.shields.io/github/downloads/LaFesta1749/ScpReplacer-Exiled/total?label=Downloads&color=333333&style=for-the-badge)](https://github.com/LaFesta1749/ScpReplacer-Exiled/releases/latest)
[![Discord](https://img.shields.io/badge/Discord-Join-5865F2?style=for-the-badge&logo=discord&logoColor=white)](https://discord.gg/PTmUuxuDXQ)

A plugin for SCP: Secret Laboratory using the Exiled framework.
Automatically handles SCP players who leave mid-round, letting others volunteer to replace them.

## 🔧 Features

- If an SCP player leaves early, human players can **volunteer** to replace them
- SCPs can **request to become Spectators or Humans** with `.spectator` and `.human` commands
- If no one volunteers, a **random D-Class** can be selected (configurable)
- Optionally force SCP role change even if no replacement exists
- Fully customizable broadcast messages
- SCP roles can be **blacklisted** from being replaced

## ⚙️ Configuration (config.yml)

```yaml
is_enabled: true
debug: false
timer: 120
lottery_timer: 15
is_specator_cmd_enabled: true
is_human_cmd_enabled: true
scientist_chance: 10
random_d_class: true
change_scp: true
blacklisted_scps:
  - Scp0492
```

## ✅ Commands

- `.spectator` – Leave SCP role and become Spectator
- `.human` – Leave SCP role and become Scientist or D-Class (based on chance)
- `.volunteer [scpname]` – Volunteer to replace a missing SCP (e.g. `.volunteer Scp173`)

## 📦 Installation

1. Download the `.dll` and place it in your `Exiled/Plugins` folder.
2. Start the server once to generate config.
3. Tweak the settings in `Exiled/Configs/port-config.yml`.

## 👤 Author

Created by **LaFesta1749**

