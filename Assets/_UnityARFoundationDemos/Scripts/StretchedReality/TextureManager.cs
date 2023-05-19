using UnityEngine;

namespace StretchedReality
{
    public class TextureManager : MonoSingleton<TextureManager>
    {
        private Texture2D[] faceFilterTextures;

        private int textureIndex = 0;

        public int TextureIndex
        {
            get
            {
                return textureIndex;
            }
            set
            {
                textureIndex = value;

                if (textureIndex == -1)
                {
                    textureIndex = faceFilterTextures.Length - 1;
                }
                else if (textureIndex == faceFilterTextures.Length - 1)
                {
                    textureIndex = 0;
                }

                SetTexture();
            }
        }

        private void Start()
        {
            faceFilterTextures = Resources.LoadAll<Texture2D>("FaceFilterTextures");
            //visualTextures = Resources.LoadAll<Texture2D>("TestTextures");

            SetTexture();
        }

        private void SetTexture()
        {
            FaceFilterManager.Instance.FaceFilterMat.SetTexture("_FaceFilterTex", faceFilterTextures[textureIndex]);
        }
    }
}