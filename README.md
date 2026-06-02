# ExpirationDate

# Vertical Slide:

[Video click aquí](https://youtu.be/dADt2uD0uK4)

# Game Pitch

[Video click aquí](https://youtu.be/gBgEwOgvfQM)

## Laboratorio 8: Input Actions (progra)

[Video click aquí](https://youtu.be/Ada4mq2WccE)


### Enlace del video del DEMO avances segunda entrega (game design):

[Video click aquí](https://youtu.be/zSqz0dnzF28)

### DEMO en Itch.io:

[Demo click aquí](https://angelargd8.itch.io/expirationdate)

### Niveles jugables:
- Restaurant
---
### Breve descripcion del proyecto:

Expiration Date es un juego de acción y supervivencia donde el jugador es una hamburguesa que sus ingredientes tienen fecha de caducidad al día siguiente. Por lo tanto, debe conseguir ingredientes frescos para evitar ser tirada al día siguiente cuando abran el restaurante. Los ingredientes los encuentra en todo el restaurante, pero para conseguirlos deberá luchar contra otras hamburguesas vencidas y hamburguesas frescas que huyen del jugador porque no quieren contaminarse. 

La experiencia combina mecánicas de movimiento en tercera persona, salto, sprint, recolección de ingredientes, lanzamiento de proyectiles, sistema de vida, frescura, enemigos con inteligencia artificial, combate y condiciones de victoria y derrota.


## Objetivo del juego

El objetivo principal del jugador es sobrevivir dentro del restaurante mientras mantiene su vida y frescura.

Para lograrlo, el jugador debe:

- Moverse por el escenario.
- Recolectar ingredientes.
- Lanzar ingredientes como proyectiles.
- Enfrentarse a hamburguesas enemigas.
- Evitar perder toda su vida o frescura.
- Llegar a la zona final para activar la victoria.

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

Este objeto se coloca como hijo del jugador y define desde dónde sale el ingrediente lanzado.

El flujo del sistema es:

1. El jugador presiona la acción Throw.
2. El script IngredientThrower recibe el input.
3. Se instancia el prefab del ingrediente en la posición del ThrowPoint.
4. Se obtiene el Rigidbody del proyectil.
5. Se aplica una fuerza hacia adelante usando la dirección del ThrowPoint.
6. El proyectil se destruye después de un tiempo determinado o al colisionar.

Esto permite que el ingrediente lanzado se comporte como un proyectil físico dentro de la escena.

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

## Barras de estado

Se implementaron barras visuales para mostrar la vida y la frescura de la hamburguesa.

Estas barras se muestran sobre el personaje y leen los valores del script:

- BurgerStats

Cada barra muestra el porcentaje actual correspondiente:

valor actual / valor máximo

Por ejemplo, si la hamburguesa tiene:

Vida máxima: 100
Vida actual: 10

la barra de vida muestra únicamente el 10%.

El sistema está pensado para poder convertirse en prefab y reutilizarse en otras hamburguesas, como enemigos o NPCs.

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
