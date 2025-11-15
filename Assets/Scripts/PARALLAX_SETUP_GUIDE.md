# Parallax Scrolling Setup Guide

This guide will walk you through setting up a 2D parallax scrolling effect with endless scrolling in your BirdyFlap game.

## Overview

The parallax system creates depth by moving background layers at different speeds. Layers farther away move slower than those closer to the camera, creating an illusion of 3D depth in your 2D game.

---

## Scene Setup Instructions

### Step 1: Create the Parallax Layer Hierarchy

1. In your scene hierarchy, create a new empty GameObject and name it `ParallaxBackgrounds`
2. Set its position to `(0, 0, 0)`
3. This will be the parent container for all parallax layers

### Step 2: Create Individual Parallax Layers

Create three child GameObjects under `ParallaxBackgrounds`:

```
ParallaxBackgrounds
├── Layer_FarBackground
├── Layer_Clouds
└── Layer_NearBackground
```

**For each layer:**
1. Right-click `ParallaxBackgrounds` → Create Empty
2. Rename it (e.g., `Layer_FarBackground`)
3. Add the `ParallaxLayer` component to it
4. Set its position to `(0, 0, 0)`

### Step 3: Add Background Segments

For each layer, you need to create 2-3 child sprite GameObjects that will seamlessly repeat:

**Example for Layer_FarBackground:**

1. Right-click `Layer_FarBackground` → 2D Object → Sprite
2. Rename it to `Segment_0`
3. In the Inspector, set the Sprite to one of your background images:
   - For far background: `background_color_hills.png` or `background_solid_sky.png`
4. **Important:** Note the width of your sprite (look at the SpriteRenderer bounds or the sprite's pixel width divided by Pixels Per Unit)
5. Duplicate this sprite 2 more times (Ctrl+D or Cmd+D)
6. Rename them `Segment_1` and `Segment_2`

**Repeat this process for all three layers.**

---

## Recommended Layer Setup

### Layer 1: Far Background (Sky/Hills)
- **Sprite:** `Assets/Art/Backgrounds/Default/background_color_hills.png` or `background_solid_sky.png`
- **Parallax Factor:** `0.1` to `0.2`
- **Sorting Layer:** Background (or -3)
- **Position Z:** `10` (far from camera)

### Layer 2: Mid Background (Clouds)
- **Sprite:** `Assets/Art/Backgrounds/Default/background_clouds.png`
- **Parallax Factor:** `0.4` to `0.5`
- **Sorting Layer:** Background (or -2)
- **Position Z:** `5` (mid distance)

### Layer 3: Near Background (Trees/Ground)
- **Sprite:** `Assets/Art/Backgrounds/Default/background_color_trees.png` or `background_fade_trees.png`
- **Parallax Factor:** `0.7` to `0.8`
- **Sorting Layer:** Background (or -1)
- **Position Z:** `2` (closer to camera)

---

## Configuring the ParallaxLayer Component

For each layer GameObject (not the segments), configure the `ParallaxLayer` component:

### Layer_FarBackground Settings:
```
Parallax Factor: 0.15
Segment Width: [measure your sprite width]
Segment Count: 3
Main Camera: [leave empty for auto-detect]
```

### Layer_Clouds Settings:
```
Parallax Factor: 0.45
Segment Width: [measure your sprite width]
Segment Count: 3
Main Camera: [leave empty for auto-detect]
```

### Layer_NearBackground Settings:
```
Parallax Factor: 0.75
Segment Width: [measure your sprite width]
Segment Count: 3
Main Camera: [leave empty for auto-detect]
```

---

## Measuring Sprite Width

To find the correct `Segment Width` value:

1. Select one of your segment sprites
2. Look at the Sprite Renderer component
3. Check the sprite's size in the Inspector (Transform or Sprite properties)
4. **Formula:** `Segment Width = Sprite Pixel Width / Pixels Per Unit`
   - Example: If your sprite is 1024 pixels wide and PPU is 100, width = 10.24 units
5. Alternatively, in the Scene view, select the sprite and look at its bounds

**Tip:** If you're not sure, start with a value like `20` and adjust based on visual gaps.

---

## Setting Up Sorting Layers (Optional but Recommended)

To ensure proper rendering order:

1. Go to **Edit → Project Settings → Tags and Layers**
2. Expand **Sorting Layers**
3. Add these layers if not present:
   - `FarBackground` (order: -3)
   - `MidBackground` (order: -2)
   - `NearBackground` (order: -1)
   - `Default` (order: 0) - for your player and obstacles

4. Assign each parallax layer's segments to the appropriate sorting layer:
   - Layer_FarBackground segments → `FarBackground`
   - Layer_Clouds segments → `MidBackground`
   - Layer_NearBackground segments → `NearBackground`

---

## Testing Your Setup

### 1. Initial Test
1. Press Play in Unity
2. Your player should start moving forward
3. Background layers should scroll at different speeds
4. Watch for any gaps or issues

### 2. Check for Gaps
If you see gaps between segments:
- **Increase** the `Segment Width` value slightly
- Or ensure your sprites are perfectly seamless
- Or adjust segment positions manually to overlap slightly (0.1 units)

### 3. Check for Overlapping
If segments overlap:
- **Decrease** the `Segment Width` value

### 4. Verify Endless Scrolling
- Let the game run for 30+ seconds
- Segments should seamlessly reposition
- No visible "jumps" or gaps should appear

---

## Tuning Parallax Speeds

The parallax factor determines how fast each layer moves relative to the camera:

- **0.0** = Static (doesn't move at all)
- **1.0** = Moves exactly with the camera (no parallax effect)

### Recommended Starting Values:
- **Far layers:** 0.1 - 0.3 (very slow, creates depth)
- **Mid layers:** 0.4 - 0.6 (moderate speed)
- **Near layers:** 0.7 - 0.9 (fast, appears close)

### Tuning Tips:
1. Start with the recommended values
2. Play the game and observe the effect
3. Adjust values in real-time (Unity allows this during Play mode)
4. Look for a natural, pleasing depth effect
5. Ensure far backgrounds move noticeably slower than near ones

### Common Adjustments:
- **Too much motion:** Reduce parallax factors across all layers
- **Not enough depth:** Increase the difference between far and near factors
- **Background moves backward:** Parallax factor is too low (below ~0.3)
- **Everything moves together:** Factors are too similar (increase variation)

---

## Advanced: Using Cinemachine

Since you're using Cinemachine 3.1.5, the parallax system will work automatically:

1. The `ParallaxLayer` script automatically finds and tracks the main camera
2. As Cinemachine moves the camera to follow your player, parallax layers update
3. No additional setup needed!

**Note:** If you have multiple cameras, you can manually assign the camera in the `ParallaxLayer` component.

---

## Troubleshooting

### Issue: Jittery/stuttering backgrounds
**Solution:**
- Enable **Rigidbody2D Interpolation** on your player GameObject
- Select your player → Inspector → Rigidbody2D → Interpolation → Set to "Interpolate"
- This smooths the position between physics updates and eliminates micro-jitter
- Note: The `MoveForward` and `Flapper` scripts now automatically enable this

### Issue: Backgrounds don't move
**Solution:**
- Ensure the camera is tagged as "MainCamera"
- Or manually assign the camera in the ParallaxLayer component
- Check that parallax factor is not 0

### Issue: Segments "jump" or have visible seams
**Solution:**
- Ensure `Segment Width` matches your sprite width exactly
- Use seamless/tileable background sprites
- Add a small overlap by adjusting segment positions manually

### Issue: Backgrounds move in the wrong direction
**Solution:**
- Check that your camera is actually moving (it should follow the player)
- Ensure parallax factor is between 0 and 1

### Issue: Performance issues
**Solution:**
- Reduce the number of segments per layer to 2
- Use smaller sprite textures
- Ensure sprites are using Sprite Atlas for batching

---

## Final Hierarchy Example

Your final scene hierarchy should look like this:

```
Scene
├── Main Camera (with CameraFollowXOnlyTarget or Cinemachine)
├── Player (with MoveForward and Flapper)
├── ParallaxBackgrounds
│   ├── Layer_FarBackground (ParallaxLayer component)
│   │   ├── Segment_0 (SpriteRenderer)
│   │   ├── Segment_1 (SpriteRenderer)
│   │   └── Segment_2 (SpriteRenderer)
│   ├── Layer_Clouds (ParallaxLayer component)
│   │   ├── Segment_0 (SpriteRenderer)
│   │   ├── Segment_1 (SpriteRenderer)
│   │   └── Segment_2 (SpriteRenderer)
│   └── Layer_NearBackground (ParallaxLayer component)
│       ├── Segment_0 (SpriteRenderer)
│       ├── Segment_1 (SpriteRenderer)
│       └── Segment_2 (SpriteRenderer)
└── [Other game objects...]
```

---

## Quick Start Checklist

- [ ] Create `ParallaxBackgrounds` parent GameObject
- [ ] Create 3 layer GameObjects as children
- [ ] Add `ParallaxLayer` component to each layer
- [ ] Create 3 sprite segments per layer
- [ ] Assign background sprites to segments
- [ ] Measure and set `Segment Width` for each layer
- [ ] Set `Parallax Factor` for each layer (0.15, 0.45, 0.75)
- [ ] Set up Sorting Layers (optional)
- [ ] Test in Play mode
- [ ] Tune parallax speeds to your liking
- [ ] Verify endless scrolling works for 30+ seconds

---

## Next Steps

Once your basic parallax is working:
- Experiment with different background combinations
- Try adding more layers for richer depth
- Add foreground parallax elements (trees, rocks) with factors above 0.9
- Combine with fog/color gradients for atmospheric effects

Happy scrolling!

