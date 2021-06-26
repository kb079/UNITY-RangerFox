Dapasa/FX/ForceShield

USO:
Para que el Shader dibuje el efecto en las intersecciones con otros objetos la cámara debe renderizar la textura de profundidad y normales.
https://docs.unity3d.com/Manual/SL-CameraDepthTexture.html

Asignarle el script "Scripts/RenderDepth.cs" a una cámara hace que esta la renderize.