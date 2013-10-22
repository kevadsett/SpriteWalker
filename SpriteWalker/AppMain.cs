using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;

namespace SpriteWalker
{
	public class AppMain
	{
		private static Walker walker;

		public static void Main (string[] args)
		{
			Director.Initialize ();
			Director.Instance.GL.Context.SetClearColor(255,255,255,0);
			
			walker = new Walker("run.png", "run.xml");

			var scene = new Scene();
			scene.Camera.SetViewFromViewport();
			SpriteTile sprite = walker.Get("walkLeft00");

			sprite.Position = scene.Camera.CalcBounds().Center;
			sprite.CenterSprite();
            sprite.Scale = new Vector2(1,1);
            scene.AddChild(sprite);
			
			Director.Instance.RunWithScene(scene, true);

			System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
			
			int spriteOffset = 0;
			timer.Start();
			bool walkLeft = true;
			while(true) {
				if(timer.ElapsedMilliseconds > 25f) {
					string spriteName;
					if(walkLeft) {
						spriteName = "walkLeft";
                        sprite.Position = new Vector2(sprite.Position.X - 10, sprite.Position.Y);
					} else {
						spriteName = "walkRight";
                        sprite.Position = new Vector2(sprite.Position.X + 10, sprite.Position.Y);
					}
                    if(sprite.Position.X < 0) {
                        walkLeft = false;
                    }
                    if(sprite.Position.X > Director.Instance.GL.Context.GetViewport().Width) {
                        walkLeft = true;
                    }
					spriteName += spriteOffset.ToString("00");

					sprite.TileIndex2D = walker.Get(spriteName).TileIndex2D;
                    
					if(spriteOffset >= 8) {
						spriteOffset = 0;
					} else {
						spriteOffset ++;
					}
					
					timer.Reset();
					timer.Start();
				}
				
				Director.Instance.Update();
				Director.Instance.Render();
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
			}
		}
		
		private static void trace(string msg) {
			System.Diagnostics.Debug.WriteLine(msg);
		}
	}
}
