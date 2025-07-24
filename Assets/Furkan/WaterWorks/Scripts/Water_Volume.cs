using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Water_Volume : ScriptableRendererFeature
{
    class WaterPass : ScriptableRenderPass
    {
        private Material waterMaterial;
        private RTHandle colorHandle;
        private RTHandle depthHandle;
        private ProfilingSampler profilingSampler = new ProfilingSampler("Water Pass");

        public WaterPass(Material material)
        {
            this.waterMaterial = material;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            // Geçerli renk ve derinlik RTHandle referanslarını al
            colorHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;
            depthHandle = renderingData.cameraData.renderer.cameraDepthTargetHandle;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (waterMaterial == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get("Water Pass");

            using (new ProfilingScope(cmd, profilingSampler))
            {
                Blit(cmd, colorHandle, colorHandle, waterMaterial);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void OnCameraCleanup(CommandBuffer cmd) { }
    }

    [System.Serializable]
    public class WaterSettings
    {
        public Material waterMaterial;
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public WaterSettings settings = new WaterSettings();
    private WaterPass waterPass;

    public override void Create()
    {
        if (settings.waterMaterial == null)
        {
            Debug.LogError("Water material not assigned!");
            return;
        }

        waterPass = new WaterPass(settings.waterMaterial)
        {
            renderPassEvent = settings.renderPassEvent
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.waterMaterial != null)
        {
            renderer.EnqueuePass(waterPass);
        }
    }
}
