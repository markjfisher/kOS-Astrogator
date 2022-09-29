# kOS-Astrogator

[Astrogator](https://github.com/HebaruSan/Astrogator) functionality in kOS.

## Usage

```
addons:astrogator:help.
```

## Creating and calculating Transfer Burns

### Creating burn maneuver nodes to a target

```
addons:astrogator:create(Mun).
```

This will create the main transfer and a plane change node, unless a second arg of `false` is given,
which will then only create the primary burn node.


### Calculating burn data (no nodes)

It is possible to get the burn data for a transfer without creating the nodes with:

```
set nodeList to addons:astrogator:calculateBurns(Mun).
print nodeList.
```

This will produce a list of `BurnModel` objects (up to 2). The first is the starting burn,
the second (if present) is the plane change burn.

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
