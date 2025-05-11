# Celeste-Style Unity Player Controller

A custom Character Controller that recreates the tight, responsive movement mechanics found in the popular platformer game [Celeste](https://store.steampowered.com/app/504230/Celeste/).

## Features

- **Precise Movement Physics**: Acceleration and deceleration that gives that snappy Celeste feel
- **Variable Jump Height**: Release the jump button early for shorter jumps
- **Jump Apex Controls**: Better control at the peak of your jump (like in Celeste)
- **Custom Gravity**: Variable gravity that increases as you fall
- **New Input System**: Built for Unity's new Input System package

## Getting Started

### Prerequisites

- Unity 2020.1 or newer
- Unity's New Input System package

### Setup

1. Add the `PlayerController.cs` script to your player GameObject
2. Ensure your player has a `CharacterController` component attached
3. Set up your Input Actions with:
   - A `move` action (Vector2 value)
   - A `Jump` action (Button value)

## Configuration

The controller has several parameters you can adjust in the Inspector:

### Walking
- **Acceleration**: How quickly the player reaches maximum speed
- **Deceleration**: How quickly the player slows down when input is released
- **Max Move Speed**: Maximum movement speed
- **Apex Bonus**: Movement speed bonus at the jump apex for better control

### Jump
- **Jump Height**: Initial velocity applied when jumping
- **Ended Jump Early Modifier**: Increased gravity when the jump button is released early
- **Jump Apex Threshold**: Vertical velocity threshold for determining the jump apex

### Gravity
- **Max Gravity**: Maximum gravity value applied during fall
- **Min Gravity**: Minimum gravity applied at the start of a fall
- **Grounded Gravity**: Small downward force applied when grounded
- **Max Fall Speed**: Maximum falling speed

## Advanced Features

### Jump Apex Physics
This controller implements the famous "jump apex" mechanic from Celeste, where you get better horizontal control at the peak of your jump. This helps with precise platforming and makes the controls feel responsive even in mid-air.

### Variable Jump Height
By releasing the jump button early, you can perform shorter jumps for precise platforming - a key feature of Celeste's movement system.

## Credits

- Inspired by the movement mechanics in [Celeste](https://store.steampowered.com/app/504230/Celeste/) by Extremely OK Games

## License

This project is available under the MIT License - feel free to use and modify!

---

*Note: This is not an official Celeste product. Celeste is a game by Extremely OK Games, Ltd.*
