# ExpirationDate

### Enlace del video:

[Video click aquí](https://youtu.be/kZIbPFBSW6U)

### Demo:

[Demo click aquí](https://angelargd8.itch.io/expirationdate?secret=dYigCBnygTMSvVDLNdYzm2B8)

### Niveles jugables:
- Restaurant

### Breve descripcion del proyecto:
Eres una hamburguesa que está por vencer, entonces será tirada al día siguiente si no cambias tus ingredientes. Por lo tanto, luego de cerrar el local debes de encontrar ingredientes frescos mientras te enfrentas contra otras hamburguesas que también buscan nuevos ingredientes y las hamburguesas frescas que no quieren contaminarse. 


## Sistemas principales implementados


## Controles

- **W, A, S, D** – Movimiento
- **Mouse** – Mirar alrededor
- **Space** – Saltar
- **Left Shift** – Correr / Sprint
- **E** – Interactuar
- **Click izquierdo o tecla asignada** – Lanzar ingrediente / tomate


### Player Controller

Se implementó un controlador para el jugador utilizando `Rigidbody`, permitiendo que la hamburguesa pueda moverse de forma física dentro del escenario.

El jugador puede:

- Moverse en dirección relativa a la cámara.
- Rotar hacia la dirección en la que camina.
- Saltar únicamente cuando está tocando el suelo.
- Correr usando la tecla Shift.
- Mirar alrededor usando el mouse.

El movimiento se maneja desde el script `PlayerMovement`, el cual utiliza la cámara como referencia para que el jugador se mueva de forma similar a un juego en tercera persona.



## Input System

El proyecto utiliza el **New Input System** de Unity para manejar las acciones del jugador.

Se creó un archivo de Input Actions con un Action Map llamado:

- Player

Dentro de este Action Map se configuraron acciones como:

- Move
- Look
- Jump
- Sprint
- Throw
- Interact
- Attack
- Crouch
- Previous
- Next

El componente `Player Input` se encarga de recibir estas acciones y enviarlas a los scripts correspondientes mediante métodos como:

- `OnMove`
- `OnLook`
- `OnJump`
- `OnSprint`
- `OnThrow`

Esto permite tener un sistema de controles más ordenado y fácil de expandir.


## Sistema de lanzamiento de ingredientes

Se implementó un sistema para que la hamburguesa pueda lanzar tomates como proyectiles.

Para esto se creó un punto de lanzamiento llamado:

- ThrowPoint

Este objeto se coloca como hijo del jugador y define desde dónde sale el ingrediente lanzado.

El flujo del sistema es:

1. El jugador presiona la acción `Throw`.
2. El script `IngredientThrower` recibe el input.
3. Se instancia el prefab del ingrediente en la posición del `ThrowPoint`.
4. Se obtiene el `Rigidbody` del proyectil.
5. Se aplica una fuerza hacia adelante usando la dirección del `ThrowPoint`.
6. El proyectil se destruye después de un tiempo determinado o al colisionar.

Esto permite que el tomate se comporte como un proyectil físico dentro de la escena.


## Scriptable Objects

El proyecto utiliza **Scriptable Objects** para definir los datos de los ingredientes.

Se creó un Scriptable Object llamado `IngredientData`, el cual permite configurar información como:

- Nombre del ingrediente.
- Prefab del proyectil.
- Fuerza de lanzamiento.
- Daño.
- Tiempo de vida.
- Ícono opcional.

Esto permite que cada ingrediente tenga sus propios datos sin tener que crear scripts diferentes para cada uno.

Por ejemplo:

- TomatoData
- CheeseData
- LettuceData

Actualmente se implementó el tomate como primer ingrediente lanzable, pero el sistema está preparado para agregar más ingredientes fácilmente.




## Assets:
- https://assetstore.unity.com/packages/3d/environments/fast-food-restaurant-kit-239419
- https://jasmine-perry.itch.io/free-burger
- http://citypng.com/photo/23014/fast-food-burger-top-view-transparent-background#google_vignette
