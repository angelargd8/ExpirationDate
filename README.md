# ExpirationDate

# Vertical Slide:

[Video click aquí](https://youtu.be/dADt2uD0uK4)

# Game Pitch

[Video click aquí](https://youtu.be/gBgEwOgvfQM)

# DEMO en Itch.io:

[Demo click aquí](https://angelargd8.itch.io/expirationdate)


## Laboratorio 8: Input Actions (progra)

[Video click aquí](https://youtu.be/Ada4mq2WccE)


### Enlace del video del DEMO avances segunda entrega (game design):

[Video click aquí](https://youtu.be/zSqz0dnzF28)


### Niveles jugables:
- Restaurant
---
### Breve descripcion del proyecto:

Expiration Date es un juego de acción y supervivencia donde el jugador controla una hamburguesa cuyos ingredientes están próximos a vencer. La historia comienza dentro de un restaurante cerrado, donde la hamburguesa debe conseguir ingredientes frescos para evitar ser tirada a la basura al día siguiente cuando el restaurante vuelva a abrir.

Durante la partida, el jugador debe recorrer el restaurante, recolectar ingredientes, lanzar proyectiles y enfrentarse a otras hamburguesas. Algunas hamburguesas enemigas están descompuestas y atacan al jugador, mientras que otras hamburguesas frescas intentan mantener distancia y atacar desde lejos.

La experiencia combina mecánicas de movimiento en tercera persona, salto, sprint, recolección de ingredientes, lanzamiento de proyectiles, sistema de vida, frescura, enemigos con inteligencia artificial, combate, pausa, efectos visuales de daño y condiciones de victoria y derrota.


## Objetivo del juego

El objetivo principal del jugador es sobrevivir dentro del restaurante mientras mantiene su vida y frescura.

Para lograrlo, el jugador debe:

- Moverse por el escenario.
- Recolectar ingredientes.
- Lanzar ingredientes como proyectiles.
- Enfrentarse a hamburguesas enemigas.
- Evitar perder toda su vida o frescura.
- Derrotar a los enemigos necesarios para completar el nivel.
- Llegar al final de la partida cumpliendo las condiciones de victoria.

----


## Sistemas principales implementados


## Controles

El proyecto utiliza un Input Actions Asset propio para manejar los controles del jugador. Cada acción principal cuenta con soporte para teclado/mouse y Gamepad/Control.

| Acción | Teclado / Mouse | Gamepad / Control |
|---|---|---|
| Move | WASD / Flechas | Left Stick |
| Look | Mouse Delta | Right Stick |
| Jump | Space | Button South |
| Sprint | Left Shift | Left Stick Press |
| Interact | F | Button West |
| Throw | E / Left Click | Right Trigger |
| Pause menu | Escape |


## Input System

El proyecto utiliza el New Input System de Unity para manejar las acciones del jugador.

Se creó un Input Actions Asset propio llamado:

- NewInputSystem



Este asset contiene un Action Map llamado:

- Player


Dentro del Action Map Player se configuraron acciones como:

- Move
- Look
- Jump
- Sprint
- Interact
- Throw

Estas acciones permiten centralizar todos los inputs del juego dentro de un mismo sistema, evitando el uso del sistema antiguo de Unity como Input.GetKeyDown o accesos directos como Keyboard.current.

El componente Player Input está asignado al jugador y utiliza el asset NewInputSystem. Los scripts del jugador obtienen las acciones directamente desde este componente y se suscriben a los eventos del New Input System.

Por ejemplo, el movimiento del jugador utiliza acciones como:

- Move
- Look
- Jump
- Sprint

Esto permite que el jugador pueda moverse, mirar alrededor, saltar y correr utilizando las acciones configuradas en el Input Actions Asset.

---

## Player Controller

Se implementó un controlador para el jugador utilizando Rigidbody, permitiendo que la hamburguesa pueda moverse físicamente dentro del escenario.

El jugador puede:

- Moverse en dirección relativa a la cámara.
- Rotar hacia la dirección en la que camina.
- Saltar únicamente cuando está tocando el suelo.
- Correr usando la acción Sprint.
- Mirar alrededor usando la acción Look.

El movimiento se maneja desde el script PlayerMovement, el cual utiliza la cámara como referencia para que el jugador se mueva de forma similar a un juego en tercera persona.

El script usa acciones del New Input System como:

- Move
- Look
- Jump
- Sprint

El movimiento se maneja desde el script:

- PlayerMovement

---

## Sistema de cámara

La cámara funciona en tercera persona y sigue a la hamburguesa utilizando un punto de referencia llamado:

- camera target

El jugador puede mover la cámara con el mouse mediante la acción:

- Look

Esta acción usa el delta del mouse para modificar la rotación horizontal y vertical de la cámara. También puede vincularse al stick derecho de un Gamepad para permitir control con mando.

---

## Sistema de salto

El jugador puede saltar usando la acción:

- Jump

El salto solo se permite cuando la hamburguesa está tocando el suelo. Para esto se usa un objeto llamado:

- GroundCheck

Este objeto verifica si el jugador está sobre una capa considerada como suelo.

---

## Sistema de sprint

El jugador puede correr usando la acción:

- Sprint

Cuando la acción está activa, el jugador cambia de su velocidad normal a una velocidad mayor.

---

## Sistema de lanzamiento de ingredientes

Se implementó un sistema para que la hamburguesa pueda lanzar ingredientes como proyectiles.

Para esto se creó un punto de lanzamiento llamado:

- ThrowPoint

Este objeto se coloca como hijo del jugador o del enemigo y define desde dónde sale el ingrediente lanzado.

El sistema utiliza el script:

- IngredientThrower

El flujo general del lanzamiento es:

1. El jugador presiona la acción Throw o el enemigo ejecuta su ataque.
2. El script IngredientThrower valida que exista un ingrediente actual.
3. Se calcula la dirección del lanzamiento.
4. Se obtiene un proyectil desde el sistema de object pooling.
5. Se posiciona el proyectil en el ThrowPoint.
6. Se le asigna un owner para evitar que dañe al personaje que lo lanzó.
7. Se limpia su velocidad previa.
8. Se aplica fuerza al Rigidbody del proyectil.
9. El proyectil se devuelve al pool al terminar su tiempo de vida o al impactar contra un objetivo válido.

Para el jugador, la dirección del lanzamiento se calcula tomando como referencia la cámara, de modo que el ingrediente se lanza hacia donde apunta el jugador.

Para los enemigos, la dirección se calcula hacia la posición del jugador, permitiendo que las hamburguesas enemigas disparen hacia su objetivo.

---

## Object Pooling de proyectiles

Se implementó object pooling para los proyectiles de ingredientes con el objetivo de mejorar el rendimiento del juego.

Antes, cada lanzamiento creaba un nuevo proyectil con `Instantiate` y luego lo eliminaba con `Destroy`. Aunque esto funcionaba, podía generar carga innecesaria si el jugador y varios enemigos lanzaban muchos ingredientes durante la partida.

Ahora el sistema reutiliza proyectiles ya creados. Cuando se necesita lanzar un ingrediente, el proyectil se toma del pool, se activa, se reposiciona y se lanza. Cuando deja de ser necesario, se desactiva y vuelve al pool para ser reutilizado más adelante.

Scripts principales del sistema:

- IngredientProjectilePool
- PooledProjectile
- IngredientThrower
- IngredientDamage

El funcionamiento del object pooling es:

1. IngredientProjectilePool mantiene una cola de proyectiles disponibles por cada prefab de ingrediente.
2. Si el pool no existe para un prefab, se crea automáticamente.
3. Si hay proyectiles disponibles, se reutiliza uno.
4. Si no hay proyectiles disponibles, se crea uno nuevo y se registra dentro del pool.
5. PooledProjectile controla el retorno automático del proyectil después de su tiempo de vida.
6. IngredientDamage devuelve el proyectil al pool cuando golpea a un jugador o enemigo válido.

Con esto se evita crear y destruir proyectiles constantemente durante el juego.

El proyectil actualmente solo desaparece inmediatamente si impacta contra un personaje con BurgerStats, como el jugador o un enemigo. Si choca contra el escenario, continúa activo hasta que termine su tiempo de vida.

---

## Sistema de daño de proyectiles

Los ingredientes lanzados pueden causar daño mediante el script:

- IngredientDamage

Cada proyectil tiene un owner, que corresponde al personaje que lo lanzó. Esto permite evitar que el proyectil dañe inmediatamente al jugador o enemigo que lo generó.

El sistema verifica si el objeto impactado tiene un componente BurgerStats. Si el objetivo tiene BurgerStats, recibe daño y el proyectil se devuelve al pool.

Esto permite que los proyectiles funcionen tanto para el jugador como para los enemigos.

---


## Sistema de pickups

Se implementó un sistema para que la hamburguesa pueda recoger ingredientes dentro del escenario.

Los ingredientes recolectables utilizan el script:

- PickableIngredient

El jugador utiliza el script:

- PlayerPickupController

El flujo del sistema es:

1. El jugador se acerca a un ingrediente.
2. Presiona la acción Interact.
3. El sistema busca ingredientes cercanos dentro de un radio de detección.
4. Se selecciona el ingrediente más cercano.
5. Se aplica el efecto del ingrediente sobre la hamburguesa.
6. El ingrediente desaparece si está configurado para destruirse al recogerlo.

Esto permite que los ingredientes no se recojan automáticamente, sino únicamente cuando el jugador presiona la acción de interacción.

---

## Sistema de vida y frescura

La hamburguesa cuenta con valores de estado como:

- Vida.
- Frescura.

Estos valores se manejan mediante el script:

- BurgerStats

La vida representa la resistencia de la hamburguesa, mientras que la frescura representa qué tan cerca está de vencer.

Los ingredientes pueden modificar estos valores. Por ejemplo:

- Un ingrediente fresco puede aumentar la frescura.
- Un ingrediente dañado puede reducir la frescura.
- Algunos ingredientes pueden recuperar vida.
- Otros pueden causar daño.

---

## Barras de estado y billboarding

Se implementaron barras visuales para mostrar la vida y la frescura de la hamburguesa.

Estas barras se muestran sobre el personaje y leen los valores del script:

- BurgerStats

Cada barra muestra el porcentaje actual correspondiente:

- valor actual / valor máximo

Por ejemplo, si la hamburguesa tiene:

- Vida máxima: 100
- Vida actual: 10

La barra de vida muestra únicamente el 10%.

Además, las barras utilizan billboarding para mantenerse orientadas hacia la cámara. Esto permite que el jugador pueda ver correctamente la vida y frescura de las hamburguesas sin importar desde qué dirección observe al personaje.

El sistema está pensado para convertirse en prefab y reutilizarse en otras hamburguesas, como enemigos o NPCs.
---

## Scriptable Objects

El proyecto utiliza Scriptable Objects para definir los datos de los ingredientes.

Se creó un Scriptable Object para configurar información de cada ingrediente, como:

- Nombre del ingrediente.
- Tipo de ingrediente.
- Prefab asociado.
- Fuerza de lanzamiento.
- Daño.
- Tiempo de vida.
- Cantidad de vida que modifica.
- Cantidad de frescura que modifica.
- Si se destruye al recogerlo.
- Ícono opcional.

Esto permite que cada ingrediente tenga sus propios datos sin tener que crear scripts diferentes para cada uno.

Ejemplos de datos configurables:

- TomatoData
- CheesePickupData
- LettuceData
- PattyData

Actualmente se implementó el tomate como ingrediente lanzable y se está trabajando en ingredientes recolectables como queso, lechuga, tomate y carne.


## Sistema de enemigos

El juego cuenta con enemigos controlados por inteligencia artificial. Estos enemigos pueden detectar al jugador, moverse por el escenario y atacarlo utilizando ingredientes como proyectiles.

Se implementaron diferentes comportamientos de enemigos:

- Enemy Burger 1: patrulla por el escenario, detecta al jugador y lo ataca cuando está dentro de su rango.
- Enemy Burger 2: mantiene distancia del jugador, intenta huir si el jugador se acerca demasiado y ataca desde lejos.

Cada enemigo tiene:

- Vida máxima
- Daño
- Rango de detección
- Rango de ataque
- Velocidad de movimiento
- Distancia de disparo
- Tiempo entre ataques
- Ingrediente que lanza


---

## Efecto visual de daño

Se implementó un efecto de post-processing utilizando Vignette para representar visualmente el estado de salud del jugador.

Cuando la vida del jugador baja de la mitad, la intensidad del Vignette comienza a aumentar gradualmente.

Mientras menos vida tenga el jugador, más fuerte se vuelve el efecto, hasta llegar a una intensidad máxima cuando la vida llega a 0.

---

## Animaciones

El jugador cuenta con animaciones controladas con el Animator.

Actualmente se utilizan animaciones para:

- Estado Idle.
- Salto del jugador.

El salto se activa mediante un parámetro Trigger desde el script de movimiento del jugador.


---

## Condiciones de victoria y derrota

La condición de victoria es que el jugador venció a todos los enemigos del nivel y su barra de frescura es lo suficientemente alta. De lo contrario, pierde. 



## Assets:
- https://assetstore.unity.com/packages/3d/environments/fast-food-restaurant-kit-239419
- https://jasmine-perry.itch.io/free-burger
- http://citypng.com/photo/23014/fast-food-burger-top-view-transparent-background#google_vignette
