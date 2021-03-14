#nullable enable
#pragma warning disable 1066
#pragma warning disable 618
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GYARTE.misc;

namespace GYARTE.manager 
{
    public interface INewDrawCall
    {
        public void Sprite(Vector2 position,
            Texture2D texture,
            Rectangle? destinationRectangle = null,
            Rectangle? sourceRectangle = null,
            Vector2? origin = null,
            float rotation = 0,
            Vector2? scale = null,
            Color? color = null,
            SpriteEffects effects = SpriteEffects.None,
            float layerDepth = 0,
            int priority = 100);
        
        public void Text( SpriteFont spriteFont,
            string text,
            Vector2 position,
            Color? color = null,
            int priority = 50, 
            TextDrawCall.Alignment alignment = TextDrawCall.Alignment.Center);
    }
    
    public class DrawManager : INewDrawCall
    {
        internal readonly SpriteBatch SpriteBatch;
        internal readonly GraphicsDevice GraphicsDevice;
        internal readonly WindowConfiguration WindowConfiguration;
        private readonly DrawQueue _drawQueue;

        public INewDrawCall NewDrawCall => this;

        public DrawManager(GraphicsDevice graphicsDevice, WindowConfiguration windowConfig)
        {
            SpriteBatch = new SpriteBatch(graphicsDevice);
            GraphicsDevice = graphicsDevice;
            WindowConfiguration = windowConfig; 
            _drawQueue = new DrawQueue(this);
        }

        void INewDrawCall.Sprite(Vector2 position, Texture2D texture, Rectangle? destinationRectangle = null,
            Rectangle? sourceRectangle = null, Vector2? origin = null, float rotation = 0, Vector2? scale = null,
            Color? color = null, SpriteEffects effects = SpriteEffects.None, float layerDepth = 0, int priority = 100)
        {
            _drawQueue.Add(new SpriteDrawCall(position, texture, destinationRectangle, sourceRectangle, origin, rotation, scale, color, effects, layerDepth, priority));
        }

        void INewDrawCall.Text(SpriteFont spriteFont, string text, Vector2 position, Color? color = null, int priority = 50, TextDrawCall.Alignment alignment = TextDrawCall.Alignment.Center)
        {
            _drawQueue.Add(new TextDrawCall(spriteFont, text, position, color, priority, alignment));
        }

        public void DrawQueuedCalls(bool considerPriority = true, List<Execution>? 
        preDrawExecutions = null,  List<Execution>? postDrawExecutions = null)
        {
            _drawQueue.Push(considerPriority, 
            preDrawExecutions, postDrawExecutions);
            _drawQueue.Clear();
        }
    }
    internal class DrawQueue : List<DrawCall> //TODO FUCK YOU TODO: STRUCT INTE CLAS BRUH
    {
        private readonly DrawManager _manager;
        
        internal DrawQueue(DrawManager manager)
        {
            _manager = manager;
        }
        
        /// <summary>
        /// Push all DrawCalls in queue to Monogame to be drawn.
        /// </summary>
        /// <param name="considerPriority">If the DrawCalls priority will considered or not</param>
        /// <param name="PreDrawExecutions">A list of lambda functions that will be executed before the queue is drawn</param>
        /// <param name="PostDrawExecutions">A list of lambda functions that will be executed after the queue is drawn</param>
        internal void Push(bool considerPriority = true, 
        List<Execution>? PreDrawExecutions = null, 
        List<Execution>? PostDrawExecutions = null)
        {
            _manager.GraphicsDevice.SetRenderTarget(_manager.WindowConfiguration.TargetResolution);
            
            _manager.SpriteBatch.Begin();
            
            if (
                PreDrawExecutions != null)
                foreach (var i in 
                PreDrawExecutions)
                    i.Invoke();
            
            IOrderedEnumerable<DrawCall> drawCalls = considerPriority 
                ? this.OrderByDescending(drawCall => drawCall.Priority) 
                : this.OrderByDescending(IndexOf);
            
            foreach (DrawCall call in drawCalls) 
                call.SubmitDraw(_manager.SpriteBatch);
            

            if (PostDrawExecutions != null)
                foreach (var i in PostDrawExecutions)
                    i.Invoke();

            _manager.SpriteBatch.End();
            
            _manager.GraphicsDevice.SetRenderTarget(null);
            
            _manager.SpriteBatch.Begin();
            

            _manager.SpriteBatch.Draw(
                _manager.WindowConfiguration.TargetResolution, 
            
                new Rectangle(
                    0, 
                    0, 
                    _manager.WindowConfiguration.WindowWidth, 
                    _manager.WindowConfiguration.WindowHeight),

                Color.White);
            _manager.SpriteBatch.End();
        }
    }
    public abstract class DrawCall
    {
        protected readonly Vector2 Position;

        public readonly int Priority;

        protected DrawCall(Vector2 position, int priority)
        {
            Position = position;

            Priority = priority;
        }

        internal abstract void SubmitDraw(SpriteBatch spriteBatch);

    }

    public  class SpriteDrawCall : DrawCall
    {
        private readonly Texture2D _texture;
        private readonly Rectangle? _destinationRectangle;
        private readonly Rectangle? _sourceRectangle;
        private readonly Vector2? _origin;
        private readonly float _rotation;
        private readonly Vector2? _scale;
        private readonly Color? _color;
        private readonly SpriteEffects _effects;
        private readonly float _layerDepth;
        
        /// <summary>Add a sprite to Draw queue to be submitted for drawing</summary>
        /// <param name="texture">A texture.</param>
        /// <param name="position">The drawing location on screen or null if <paramref name="destinationRectangle"> is used.</paramref></param>
        /// <param name="destinationRectangle">The drawing bounds on screen or null if <paramref name="position"> is used.</paramref></param>
        /// <param name="sourceRectangle">An optional region on the texture which will be rendered. If null - draws full texture.</param>
        /// <param name="origin">An optional center of rotation. Uses <see cref="P:Microsoft.Xna.Framework.Vector2.Zero" /> if null.</param>
        /// <param name="rotation">An optional rotation of this sprite. 0 by default.</param>
        /// <param name="scale">An optional scale vector. Uses <see cref="P:Microsoft.Xna.Framework.Vector2.One" /> if null.</param>
        /// <param name="color">An optional color mask. Uses <see cref="P:Microsoft.Xna.Framework.Color.White" /> if null.</param>
        /// <param name="effects">The optional drawing modifier. <see cref="F:Microsoft.Xna.Framework.Graphics.SpriteEffects.None" /> by default.</param>
        /// <param name="layerDepth">An optional depth of the layer of this sprite. 0 by default.</param>
        /// <param name="priority">Priority for sprite to be drawn (Lowest goes first, I.E. first drawn)</param> 
        /// <exception cref="T:System.InvalidOperationException">Throws if both <paramref name="position" /> and <paramref name="destinationRectangle" /> been used.</exception>
        /// <remarks>This overload uses optional parameters. This overload requires only one of <paramref name="position" /> and <paramref name="destinationRectangle" /> been used.</remarks>
        public SpriteDrawCall(
            Vector2 position,
            Texture2D texture,
            Rectangle? destinationRectangle = null,
            Rectangle? sourceRectangle = null,
            Vector2? origin = null,
            float rotation = 0,
            Vector2? scale = null,
            Color? color = null,
            SpriteEffects effects = SpriteEffects.None,
            float layerDepth = 0,
            int priority = 100)
            : base(position, priority)
        {
            _texture = texture;
            _destinationRectangle = destinationRectangle;
            _sourceRectangle = sourceRectangle;
            _origin = origin;
            _rotation = rotation;
            _scale = scale;
            _color = color;
            _effects = effects;
            _layerDepth = layerDepth;
        }
        
        internal override void SubmitDraw(SpriteBatch spriteBatch)
        {   
            Rectangle __destinationRectangle = _destinationRectangle == null ? new Rectangle((int)Position.X, (int)Position.Y, (int) (_scale == null? 1 : _scale.Value.X) * (_sourceRectangle == null? _texture.Bounds.Width : _sourceRectangle.Value.Width), (int) (_scale == null? 1 : _scale.Value.X) * (_sourceRectangle == null? _texture.Bounds.Height : _sourceRectangle.Value.Height)) : (Rectangle) _destinationRectangle;
            // CURSED LINJE 
            Color __color = _color == null ? Color.White : (Color) _color;
            spriteBatch.Draw(_texture, __destinationRectangle, _sourceRectangle, __color);
            

            /*
            spriteBatch.Draw(
                _texture,
                Position, _destinationRectangle,
                _sourceRectangle,
                _origin,
                _rotation,
                _scale,
                _color,
                _effects,
                _layerDepth);
            */

        }
    }

    public class TextDrawCall : DrawCall
    {
        private readonly SpriteFont _spriteFont;
        private readonly string _text;
        private readonly Color _color;
        private Alignment _alignment;
        
        public enum Alignment { Center=0, TopLeft=1}


        /// <summary>
        /// Submit a text string of sprites for drawing in the current batch.
        /// </summary>
        /// <param name="spriteFont">A font.</param>
        /// <param name="text">The text which will be drawn.</param>
        /// <param name="position">The drawing location on screen.</param>
        /// <param name="color">A color mask.</param>
        /// <param name="priority">Priority for text to be drawn (Lowest goes first, I.E. first drawn)</param>
        /// <param name="alignment">Alignment</param>
        public TextDrawCall(
            SpriteFont spriteFont,
            string text,
            Vector2 position,
            Color? color = null,
            int priority = 50, 
            Alignment alignment = Alignment.Center)
            : base(position, priority)
        {
            _spriteFont = spriteFont;
            _text = text;
            _color = color ?? Color.White;
            _alignment = alignment;

        }
        
        internal override void SubmitDraw(SpriteBatch spriteBatch)
        {
            Vector2 size = _spriteFont.MeasureString( _text );
            Vector2 position;
                
            switch (_alignment)
            {
                case Alignment.Center:
                    position = new Vector2(Position.X - size.X/2, Position.Y - size.Y/2);
                    break;
                
                case Alignment.TopLeft:
                    position = Position;
                    break;
                
                default:
                    position = Position;
                    break;
            }
            
            spriteBatch.DrawString(
                _spriteFont,
                _text,
                position,
                _color);
        }
    }
}