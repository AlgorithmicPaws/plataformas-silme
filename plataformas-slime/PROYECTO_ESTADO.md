# Estado del Proyecto — plataformas-slime

> Informe generado para onboarding rápido en nuevos chats. Última actualización: 2026-04-22.

---

## Resumen

Juego 2D platformer en Unity. Personaje tipo slime que se mueve, salta, dispara proyectiles y puede pisar enemigos. Tiene dos niveles jugables y un Main Menu funcional.

- **Motor:** Unity (URP, Universal Render Pipeline)
- **Input:** New Input System (`com.unity.inputsystem 1.14.2`)
- **UI:** TextMeshPro
- **Cámara:** Cinemachine instalado pero sin configurar aún
- **Plataforma destino:** PC

---

## Escenas (Build Order)

| Índice | Archivo | Estado |
|--------|---------|--------|
| 0 | `Assets/Scenes/MainMenu.unity` | Completo — botones Nueva Partida y Salir funcionando |
| 1 | `Assets/Scenes/GrassLevel.unity` | Nivel jugable activo |
| 2 | `Assets/Scenes/CastleLevel.unity` | Nivel jugable activo |

---

## Input (New Input System)

**Archivo:** `Assets/Settings/PlayerActions.inputactions`  
**Action Map:** `Player`

| Acción | Tecla | Tipo |
|--------|-------|------|
| Move | A / D | Value (1D Axis, float) |
| Jump | Space | Button |
| Shoot | S | Button |

---

## Estructura de Scripts — `Assets/_Scripts/`

### Player (todos en el mismo GameObject)

**`NewInput.cs`** — clase `NewInput : MonoBehaviour`
```
Campos:
  [HideInInspector] public float inputX   ← leído por otros scripts
  private PlayerInput playerInput
  private PlayerJump _playerJump

Métodos:
  GetInput()     → inputX = actions["Move"].ReadValue<float>()
  Jump(context)  → si context.started llama _playerJump.Jump()
```

**`PlayerMovement.cs`** — clase `PlayerMovement1 : MonoBehaviour`
```
Campos:
  public float speed
  private NewInput _newInput
  private Rigidbody2D _rigidbody

Métodos:
  PlayerMove() → _rigidbody.velocity = new Vector2(inputX * speed, velocity.y)
  Flip()       → rota transform en Y (0 o 180) según signo de inputX
```

**`PlayerJump.cs`** — clase `PlayerJump : MonoBehaviour`
```
Campos:
  public float jumpForce
  private Rigidbody2D _rigidbody

Métodos:
  Jump() → AddForce(Vector2.up * jumpForce, Impulse) solo si |velocity.y| < 0.01f

NOTA: Tiene llaves desbalanceadas (quirk de sintaxis en línea 18-23), funciona igual.
```

**`PlayerAttack.cs`** — clase `PlayerAttack : MonoBehaviour`
```
Campos:
  public float bounceForce
  private Rigidbody2D _rb

Eventos:
  OnTriggerEnter2D → si tag == "Enemy":
    - AddForce hacia arriba (rebote al player)
    - SetBool("dieAnim", true) en Animator del enemy
    - Destroy(enemy, 0.5f)

NOTA: Requiere un Trigger Collider separado en el player (normalmente en los pies).
```

**`PlayerShoot.cs`** — clase `PlayerShoot : MonoBehaviour`
```
Campos:
  public GameObject bullet      ← prefab asignado en Inspector
  public Transform shootPos     ← punto de spawn asignado en Inspector

Métodos:
  Shoot(context) → si context.started: Instantiate(bullet, shootPos) + Destroy tras 1s
```

**`PlayerHealth.cs`** — clase `PlayerHealth : MonoBehaviour`
```
Campos:
  public int maxHealth
  [HideInInspector] public int currentHealth
  private Animator animator

Métodos:
  Damage()          → llamado desde EnemyAttack; si HP > 1 inicia coroutine, si no Destroy(player)
  ReceibeDamage()   → (coroutine) HP--, isDamage=true, ignora colisión layer 3 vs 6 por 2s

Layers usados:
  Layer 3 = Player
  Layer 6 = Enemy
```

**`PlayerAnim.cs`** — clase `PlayerAnim : MonoBehaviour`
```
VACÍO — placeholder sin implementar
```

---

### Animaciones — `Assets/_Scripts/Animations/Player/`

**`PlayerAnimations.cs`** — clase `PlayerAnimations : MonoBehaviour`
```
Campos:
  private Animator _animator
  private NewInput _input
  private SpriteRenderer _spriteRenderer
  private Rigidbody2D _rigidbody

Métodos (llamados cada Update):
  PlayerMoving()  → SetBool("isMoving", inputX != 0)
  PlayerJumping() → SetFloat("isJumping", Abs(velocity.y))
```

---

### Enemigos — `Assets/_Scripts/`

**`EnemyAttack.cs`** — clase `EnemyAttack : MonoBehaviour`
```
Eventos:
  OnCollisionEnter2D → si tag == "Player": llama PlayerHealth.Damage()

NOTA: Usa OnCollisionEnter2D (colisión física), no trigger.
```

---

### Objetos — `Assets/_Scripts/Objects/`

**`Bullet.cs`** — clase `Bullet : MonoBehaviour`
```
Campos:
  public float speed
  private Rigidbody2D _rb

Eventos:
  OnEnable()          → _rb.velocity = transform.right * speed
  OnTriggerEnter2D    → si tag == "Enemy": Destroy(enemy, 0.1f)

ADVERTENCIA: Viaja en world-right, NO relativo a la dirección del player.
Si el player mira a la izquierda, la bala igualmente sale hacia la derecha.
```

---

### Escenas — `Assets/_Scripts/`

**`SceneManager.cs`** — clase `SceneLoader : MonoBehaviour`
```
Métodos públicos (llamados desde botones UI):
  StartGame() → SceneManager.LoadScene(1)
  Quit()      → Application.Quit()
```

---

## Animator Parameters

### Player (`Assets/Animations/Player/Player_AC.controller`)

| Parámetro | Tipo | Seteado por |
|-----------|------|-------------|
| `isMoving` | bool | `PlayerAnimations.PlayerMoving()` |
| `isJumping` | float | `PlayerAnimations.PlayerJumping()` |
| `isDamage` | bool | `PlayerHealth.ReceibeDamage()` |
| `dieAnim` | bool | `PlayerAttack.OnTriggerEnter2D()` (en enemy) |

### Enemy (`Assets/Animations/Enemy/Enemy.controller`)

| Parámetro | Tipo | Seteado por |
|-----------|------|-------------|
| `dieAnim` | bool | `PlayerAttack.OnTriggerEnter2D()` |

---

## Prefabs

| Prefab | Ruta | Descripción |
|--------|------|-------------|
| Bullet | `Assets/Prefab/Bullet.prefab` | Proyectil del player — tiene `Bullet.cs` y Rigidbody2D |

---

## Main Menu (estado actual)

Jerarquía de la escena `MainMenu.unity`:
```
MainMenu (Scene)
├── Main Camera
├── EventSystem
├── GameManager          ← SceneLoader.cs
└── Canvas
    ├── Background       ← Image con sprite de fondo
    ├── TitleText        ← TextMeshPro, nombre del juego
    ├── ButtonNewGame    ← OnClick → SceneLoader.StartGame()
    │   └── Text (TMP)  ← "Nueva Partida", texto transparente con color tint en hover
    └── ButtonQuit       ← OnClick → SceneLoader.Quit()
        └── Text (TMP)  ← "Salir", mismo sistema
```

**Configuración de botones:**
- Fondo de botón: transparente
- `Target Graphic` del Button apunta al **Text (TMP)** (no a la Image)
- El Color Tint se aplica directamente al texto para el efecto hover
- Fuente personalizada: importada como `.ttf` → convertida a TMP Font Asset via Window → TextMeshPro → Font Asset Creator

---

## Asset Packs incluidos (sin modificar)

| Pack | Ruta | Scripts útiles |
|------|------|----------------|
| Cainos Village Props | `Assets/Cainos/` | `MovingPlatform.cs`, `BoundingPlatform.cs`, `Elevator.cs` |
| 2D Adventure Background Pack | `Assets/2D Adventure Background Pack/` | Solo sprites, sin scripts |
| Pixel Adventure 1 | `Assets/Pixel Adventure 1/` | Solo sprites |
| Anogame MonsterGirls | `Assets/Anogame/` | Sin uso en gameplay principal |
| Slime demo | `Assets/Slime/` | Prefabs de animación de referencia |

---

## Flujo de Combate Completo

```
Player pisa enemy
  └── PlayerAttack.OnTriggerEnter2D (trigger en pies del player)
        ├── _rb.AddForce(up * bounceForce)   → player rebota
        ├── enemy.Animator.SetBool("dieAnim", true)
        └── Destroy(enemy, 0.5f)

Player dispara (S)
  └── PlayerShoot.Shoot → Instantiate(bullet) → Destroy(bullet, 1s)
        └── Bullet.OnTriggerEnter2D con enemy
              └── Destroy(enemy, 0.1f)

Enemy toca player
  └── EnemyAttack.OnCollisionEnter2D
        └── PlayerHealth.Damage()
              ├── HP > 1 → ReceibeDamage() coroutine
              │     ├── HP--
              │     ├── isDamage = true (anim)
              │     ├── IgnoreLayerCollision(3,6,true) por 2s
              │     └── IgnoreLayerCollision(3,6,false) + isDamage=false
              └── HP <= 1 → Destroy(player)
```

---

## Problemas Conocidos / Deuda Técnica

| Problema | Archivo | Impacto |
|----------|---------|---------|
| Bullet va siempre a la derecha | `Bullet.cs` línea 19 | Si player mira izquierda, la bala va en dirección incorrecta |
| Jump sin ground check real | `PlayerJump.cs` | Solo verifica `velocity.y < 0.01f`, permite doble salto en bordes |
| `PlayerAnim.cs` vacío | `PlayerAnim.cs` | Placeholder sin implementar |
| Parámetros de Animator como strings | `PlayerAnimations.cs`, `PlayerAttack.cs` | Sin impacto en runtime, pero frágil ante renombrado |
| Invincibilidad solo funciona en layer 3 vs 6 | `PlayerHealth.cs` | Nuevos tipos de enemigos en otras layers no activarán invincibilidad |
| `currentHealth` nunca llega a 0 | `PlayerHealth.cs` | La condición es `> 1`, así que muere con HP = 1 restante |

---

## Packages Unity Relevantes

```
com.unity.inputsystem          1.14.2   ← New Input System (en uso)
com.unity.render-pipelines.universal  14.0.12  ← URP
com.unity.cinemachine          2.10.7   ← Instalado, SIN configurar aún
com.unity.textmeshpro          3.0.7    ← En uso en UI
com.unity.feature.2d           2.0.1    ← Herramientas 2D
com.unity.timeline             1.7.7    ← Instalado, sin uso activo
```

---

## Próximas Mecánicas Pendientes

_(registrar aquí lo que se acuerde implementar)_

- [ ] ...

---

## Notas de Arquitectura para Nuevas Mecánicas

- **Agregar un nuevo tipo de ataque:** Crear script nuevo en `Assets/_Scripts/Player/`, agregar input action en `PlayerActions.inputactions`, referenciar desde `NewInput.cs`
- **Agregar un nuevo enemigo:** Crear prefab con tag `Enemy` en layer 6, agregar `EnemyAttack.cs` y un Animator con parámetro `dieAnim`
- **Agregar UI en gameplay:** Crear Canvas en la escena del nivel, usar TextMeshPro, leer `PlayerHealth.currentHealth` para mostrar HP
- **Configurar Cinemachine:** Agregar `CinemachineVirtualCamera` en escena, apuntar Follow/LookAt al player
- **Dirección del disparo:** Para que la bala siga la dirección del player hay que leer `NewInput.inputX` en `PlayerShoot` y ajustar la rotación de `shootPos` o de la bala al instanciar
