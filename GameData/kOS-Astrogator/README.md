# kOS-Astrogator

[Astrogator](https://github.com/HebaruSan/Astrogator) functionality in kOS.

## Usage

```
addons:astrogator:help.
```

## Transfers

### `create(body, [shouldCreatePlanarChangeNode = true])` - Creating burn maneuver nodes to a target

```
addons:astrogator:create(Mun).
```

This will create the main transfer and a plane change node, unless a second arg of `false` is given,
which will then only create the primary burn node.


### `calculateBurns(body)` - Calculating burn data (no nodes)

This will calculate the burn data for a transfer without creating the , and return a list of BurnModel structures (up to 2)

```
set bms to addons:astrogator:calculateBurns(Mun).
print bms.
print bms[0]:prograde.
```

This will produce a list of `BurnModel` objects (up to 2). The first (if present) is the starting burn,
the second (if present) is the plane change burn. See below for more information about BurnModel.

## Physics

The following functions are exposed from Astrogator's PhysicsTools

- `deltaVToOrbit(body)` Calculate deltav to get a sensible orbit around body.
- `speedAtPeriapsis(body, apo, peri)` Generically calculate the speed at periapsis around given body with this apo/peri values in its orbit.
- `speedAtApoapsis(body, apo, peri)` Generically calculate the speed at apoapsis around given body with this apo/peri values in its orbit.
- `shipSpeedAtPeriapsis()` Ship speed at periapsis around current body.
- `shipSpeedAtApoapsis()` Ship speed at apoapsis around current body.


## Structures

### BurnModel

This is a class representing the Burn without having to create a node.

It contains fields `atTime`, `prograde`, `normal`, `radial`, `totalDV`, `duration`.

The function `toNode()` will create an ingame node from the data.

Example usage:
```
set nodeList to addons:astrogator:calculateBurns(Mun).
print nodeList[0].
set dv0 to nodeList[0]:totalDV.
set newNode to nodeList[0]:toNode().
```
