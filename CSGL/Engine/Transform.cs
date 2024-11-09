using System;
using System.Text.Json;
using OpenTK.Mathematics;

namespace CSGL
{
	public class Transform : Component
	{
		public Vector3 Position 
		{
			get 
			{
				return WorldPosition; 
			}

			set
			{
				WorldPosition = value;
			}
		}

		public Quaternion Rotation;
		public Vector3 Scale;

		private Transform? _parent;
		public Transform? Parent
		{
			get => _parent;
			set
			{
				if (_parent != null)
					_parent.Children.Remove(this);

				_parent = value;
				_parent.Children.Add(this);
			}
		}

		public Vector3 LocalPosition { get; set; } = Vector3.Zero;
		public Quaternion LocalRotation { get; set; } = Quaternion.Identity;
		public Vector3 LocalScale { get; set; } = Vector3.One;


		public List<Transform> Children { get; private set; } = new List<Transform>();

		public Transform(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			Position = position;
			Rotation = rotation;
			Scale = scale;
		}

		public Transform()
		{
			this.Position = Vector3.Zero;
			this.Rotation = Quaternion.Identity;
			this.Scale = Vector3.One;
		}

		public Vector3 Forward
		{
			get
			{
				float radY = MathHelper.DegreesToRadians(LocalRotation.ToEulerAngles().Y);
				float radX = MathHelper.DegreesToRadians(LocalRotation.ToEulerAngles().X);

				return new Vector3(
					(float)(MathF.Cos(radY) * MathF.Cos(radX)),
					(float)(MathF.Sin(radX)),
					(float)(MathF.Sin(radY) * MathF.Cos(radX))
					);
			}
		}

		public Vector3 Right
		{
			get
			{
				return Vector3.Cross(Forward, Vector3.UnitY).Normalized();
			}
		}

		public Vector3 WorldPosition
		{
			get
			{
				if (Parent == null) return LocalPosition;
				return Parent.WorldPosition + Vector3.Transform(LocalPosition, Parent.Rotation);
			}
			set
			{
				if (Parent == null)
				{
					LocalPosition = value;
				}
				else
				{
					LocalPosition = Vector3.Transform(value - Parent.WorldPosition, Quaternion.Invert(Parent.Rotation));
				}
			}
		}

		public Quaternion WorldRotation
		{
			get
			{
				if (Parent == null)
					return LocalRotation;

				return Parent.WorldRotation * LocalRotation;
			}
			set
			{
				if (Parent == null)
				{
					LocalRotation = value;
				}
				else
				{
					LocalRotation = Quaternion.Invert(Parent.WorldRotation) * value;
				}
			}
		}

		public Matrix4 WorldTransform()
		{
			Matrix4 localTransform = MathU.TRS(this.LocalPosition, this.LocalRotation, this.LocalScale);

			if (this.Parent != null)
			{
				return Parent.WorldTransform() * localTransform;
			}
			else
			{
				return localTransform;
			}
		}

		public Matrix4 Matrix = Matrix4.Identity;

		public void UpdateTransforms()
		{
			this.Matrix = Matrix4.Identity;

			this.Matrix *= Matrix4.CreateFromQuaternion(this.LocalRotation);
			this.Matrix *= Matrix4.CreateTranslation(this.LocalPosition);
			this.Matrix *= Matrix4.CreateScale(this.LocalScale);

			if (this.Parent != null)
				this.Matrix *= this.Parent.Matrix;

			foreach (Transform child in Children)
			{
				child.UpdateTransforms();
			}
		}

		public void RotateAround(Vector3 point, float angle, Vector3 axis)
		{
			Vector3 offset = LocalPosition - point;

			Quaternion rotation = Quaternion.FromAxisAngle(axis.Normalized(), MathHelper.DegreesToRadians(angle));

			offset = Vector3.Transform(offset, rotation);

			LocalPosition = point + offset;
			LocalRotation = rotation * LocalRotation;
		}

		public override void Instance(Monobehaviour parent, Dictionary<string, JsonElement> serialized)
		{

		}
	}
}
