using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;

using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace SpriteWalker
{
	public class Walker
	{
		private TextureInfo _textureInfo;
		private Texture2D _texture;
		private Dictionary<string, Vector2i> _sprites;
		
		public Walker (string imageFilename, string imageDetailsFilename)
		{
			XDocument doc = XDocument.Load ("/Application/" + imageDetailsFilename);
			var lines = from sprite in doc.Root.Elements("sprite")
				select new
				{
					Name = sprite.Attribute("n").Value,
					X1 = (int)sprite.Attribute("x"),
					Y1 = (int)sprite.Attribute("y"),
					Height = (int)sprite.Attribute("h"),
					Width = (int)sprite.Attribute("w")
				};
			_sprites = new Dictionary<string, Vector2i>();
			foreach(var curLine in lines) {
                //trace ("  w = " + curLine.Width + ", h = " + curLine.Height);
				_sprites.Add(curLine.Name, new Vector2i((curLine.X1/curLine.Width), 5 - (curLine.Y1/curLine.Height)));
				//trace ("" + _sprites.ToArray()[_sprites.ToArray().Length - 1]);
			}
			_texture = new Texture2D("/Application/" + imageFilename, false);
			_textureInfo = new TextureInfo(_texture, new Vector2i(3, 6));
		}
		
		~Walker() {
			_texture.Dispose();
			_textureInfo.Dispose();
		}
		
		public SpriteTile Get(int x, int y) {
			var spriteTile = new SpriteTile(_textureInfo);
			spriteTile.TileIndex2D = new Vector2i(x, y);
            trace (spriteTile.TileIndex2D.ToString());
			spriteTile.Quad.S = new Sce.PlayStation.Core.Vector2(160, 160);
			return spriteTile;
		}
		
		public SpriteTile Get(string name) {
            trace (name);
			return Get (_sprites[name].X, _sprites[name].Y);
		}
		
		private static void trace(string msg) {
			System.Diagnostics.Debug.WriteLine(msg);
		}
	}
}

