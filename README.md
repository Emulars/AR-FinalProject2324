# AR-FinalProject2324

## Asset links
- [RPG - Tortoise Boss Pack](https://assetstore.unity.com/packages/3d/characters/creatures/rpg-tortoise-boss-pack-85267)
- [Awesome Stylized Mage Tower](https://assetstore.unity.com/packages/3d/environments/fantasy/awesome-stylized-mage-tower-53793)
- [Stone Table](https://assetstore.unity.com/packages/3d/props/stone-table-231869)
- [Cracked stone filled with lava.](https://assetstore.unity.com/packages/2d/textures-materials/floors/cracked-stone-filled-with-lava-8294)
- [Stylized Stones Texture](https://assetstore.unity.com/packages/2d/textures-materials/floors/stylized-stones-texture-153460)
- [School assets](https://assetstore.unity.com/packages/3d/environments/school-assets-146253)

## AR Foundation setup process

1. Create a new 3D Unity project.
2. Go to Window -> Package Manager.
3. Packages set to `Unity Registry`.
4. Install `AR Foundation` and `ARCore XR Plugin`.

5. For Android build,
    - Go to File -> Build Settings.
    - Select Android and click on `Switch Platform`.
    - Go to Edit -> Project Settings -> XR-Plugin Management.
        - In the Android tab, enable Google ARCore
        - In the left-hand pane, click the Player section
        - In the Android tab, under Other settings > Auto Graphics API (unchecked), remove  Vulkan from the list of Graphics APIs.
        - In the Android tab, under Other settings > Minimum API Level, select Android 7.0 'Nougat' (API level 24) or higher.

6. For Desktop build,
    - Enable XR Simulation in the Editor.
        - Go to Edit -> Project Settings -> XR-Plugin Management.
        - In the Windows tab, enable Windows XR Plugin.
    - Select an XR Simulation environment
        - Go to Window > XR > AR Foundation > XR Environment
        - Select an environment from the dropdown list.


[AR Foundation project tutorial](https://codelabs.developers.google.com/arcore-unity-ar-foundation?hl=en#3)
