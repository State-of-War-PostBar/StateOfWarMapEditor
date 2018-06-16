# Radiac UI in-Unity usage

1. You need a MainCamera in scene (just like the original unity does.)
1. A game object that shall not be removed, mounted with script RadiacEnvironment.
1. Drag and drop prefabs in UIElements folders.
1. Custom signals for UI reactions. All lists variables named *Signal xxx* is the signal it emits when something happend. *xxx Signal* is the action should be done with the specific signal.
1. Each UI element that can react with the mouse, will always have an area to react with. Use RadiacAuxiliaryArea to build up shapes that is not a rectangle, and set *use base rect* to use the rectangle defined by original rect transforms.
1. Custom your own UI componenets, or change the original components by changing scripts under UIComponents folder.
