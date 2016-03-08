using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
namespace WindowsGame1
{
    class Hazard
    {
        public Model model = null;
        public Matrix projection;//投影变换矩阵；
        public Matrix view;//观察变换矩阵
        public float haloc;
        public float hadiameter;
        public BoundingSphere hazardsphere;//汽车包围球
        
      public  float right; 
        public  bool ri;
       
        public void zuoyou(){
            Random mRandom = new Random();
            right =(float) mRandom.NextDouble();
            if (right < 0.5)
                ri = true;
            else
                ri = false;
            if (ri)
                haloc = -2.5f;
            else
                haloc = 2.5f;
    }
        public Model HazardModel
        {
          
            set
            {
                zuoyou();
                model = value;
                hazardsphere = model.Meshes[0].BoundingSphere; // 得到包含第一个部件的球体
                for (int i = 1; i < model.Meshes.Count; i++)
                {
                    //循环后，CarBoundingSphere是包含hazard所有部件的球体
                    hazardsphere = BoundingSphere.CreateMerged(hazardsphere, model.Meshes[i].BoundingSphere); //得到一个球包含参数指定的两个球
                }

                hazardsphere.Radius *= 0.09f;					// CarDraw方法的世界变换中车尺寸放大0.85倍
                hazardsphere.Center.Z =60f;				// CarDraw方法的世界变换中车位置改变
                hadiameter = hazardsphere.Radius * 2;			// 车直径=(2*半径)
                hazardsphere.Center.X = haloc;
                hazardsphere.Center.Y = 0f;
            }
        }
       
       
        public void HazardDraw(GameTime gametime,float xpositon)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();//默认灯光和材质
                    effect.World = Matrix.CreateScale(0.08f, 0.08f, 0.08f) * Matrix.CreateTranslation(haloc, 3.5f, xpositon);//汽车位置
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }
        public void addHazard(GameTime gametime)
        {
            /*float right; 
            bool ri;
            Random mRandom = new Random();
            right =(float) mRandom.NextDouble();
            if (right < 0.5)
                ri = true;
            else
                ri = false;
            if (ri)
                haloc = -2.5f;
            else
                haloc = 2.5f;*/
            this.HazardDraw(gametime,60f);
        }
        public void hazardmove(GameTime gametime,double xpositionc)
        {
            if (xpositionc > -20)
            {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();//默认灯光和材质
                        effect.World = Matrix.CreateScale(0.08f, 0.08f, 0.08f) * Matrix.CreateTranslation(haloc, 1.5f, 60f - (float)xpositionc);//汽车位置
                        effect.View = view;
                        effect.Projection = projection;
                    }
                    mesh.Draw();
                }
            }
        }
        public void addHazard()
        {
          /*  float right;
            bool ri;
            Random mRandom = new Random();
            right = (float)mRandom.NextDouble();
            if (right < 0.5)
                ri = true;
            else
                ri = false;
            if (ri)
                haloc = -2.5f;
            else
                haloc = 2.5f;*/
            this.HazardDraw( 60f);
        }
        public void HazardDraw( float xpositon)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();//默认灯光和材质
                    effect.World = Matrix.CreateScale(0.08f, 0.08f, 0.08f) * Matrix.CreateTranslation(2.5f, 1.5f, 60f);//汽车位置
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }
    }
}
