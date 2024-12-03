using Logging;
using CSGL.Engine;
using OpenTK.Mathematics;
using CSGL.Assets;
using CSGL.Graphics;
using ContentPipeline;
using SharedLibrary;
using OpenTK.Graphics.OpenGL;

namespace CSGL
{
	public class InstanceScene : Scene
	{

		public InstanceScene(string name) : base(name)
		{
			SceneManager.ActiveScene = this;
		}

		List<Matrix4> transformList = new List<Matrix4>();

		Shader shader;
		VAO vao;
		EBO ebo;
		VBO vbo;
		VBO ibo;

		int numInstances = 100;

		int vec4Size = 4 * sizeof(float);
		int mat4Size = 16 * sizeof(float);

		void InitScene()
		{
			MainLight = new Light(Color4.White, 0.2f, 1f, 0.2f, 16f);
			Box box = new Box();

			box.transform.position = new Vector3(0, 0, 0);
			this.renderScene.Add(box);
		}

		void InitBuffers()
		{
			shader = ShaderManager.Shaders["instance.shader"];

			ModelImporter importer = new ModelImporter(Manifest.GetAsset<ModelAsset>("cube.obj").FilePath);

			List<Matrix4> transforms = new List<Matrix4>();

			transforms.Add(Matrix4.CreateTranslation(0, 0, 0));

			vao = new VAO();
			vao.Bind();

			vbo = new VBO(importer.meshes[0].Vertices);
			ibo = new VBO(transforms);

			ebo = new EBO(importer.meshes[0].Indices);

			vao.LinkAttrib(vbo, 0, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.PositionOffset);
			vao.LinkAttrib(vbo, 1, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.NormalOffset);
			vao.LinkAttrib(vbo, 2, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.TangentOffset);
			vao.LinkAttrib(vbo, 3, 2, VertexAttribPointerType.Float, Vertex.Stride, Vertex.UVOffset);

			int vec4size = Vector4.SizeInBytes;
			int mat4Size = 4 * vec4size;

			vao.LinkAttrib(ibo, 4, 4, VertexAttribPointerType.Float, mat4Size, 0);
			vao.LinkAttrib(ibo, 5, 4, VertexAttribPointerType.Float, mat4Size, 1 * vec4size);
			vao.LinkAttrib(ibo, 6, 4, VertexAttribPointerType.Float, mat4Size, 2 * vec4size);
			vao.LinkAttrib(ibo, 7, 4, VertexAttribPointerType.Float, mat4Size, 3 * vec4size);

			GL.VertexAttribDivisor(4, 1);
			GL.VertexAttribDivisor(5, 1);
			GL.VertexAttribDivisor(6, 1);
			GL.VertexAttribDivisor(7, 1);

			ErrorCode error = GL.GetError();

			Log.GL(error.ToString());

			vao.Unbind();
			vbo.Unbind();
			ibo.Unbind();
			ebo.Unbind();
		}

		public override void Awake()
		{
			Log.Info($"{base.Name}({base.sceneID}) Scene Awake");

			//Mesh mesh = new Mesh(test.m)

			base.Awake();
		}

		public override void Start()
		{
			InitScene();
			MainLight.transform.position = new Vector3(0.0f, 0.5f, 0.0f);
			MainLight.transform.scale = Vector3.One * 0.05f;

			Camera.main.Yaw = 270;
			Camera.main.Pitch = -65.5f;
			Camera.main.transform.position = new Vector3(0, 10, 5);

			for (int i = 0; i < numInstances; i++)
			{
				float x = MathU.Random(0, 100);
				float y = MathU.Random(0, 100);
				float z = MathU.Random(0, 100);

				Vector3 pos = new Vector3(x, y, z);
				transformList.Add(Matrix4.CreateTranslation(pos));
			}

			InitBuffers();

			base.Start();
		}

		float t = 0;
		public override void Update()
		{
			t += 1;

			MainLight.transform.position += new Vector3(MathF.Cos(MathU.Rad(t)), MathF.Sin(MathU.Rad(t+10)), MathF.Sin(MathU.Rad(t))) * Time.deltaTime;

			base.Update();
		}

		public override void FixedUpdate()
		{		
			base.FixedUpdate();
		}

		public override void Render()
		{
			shader.Activate();

			vao.Bind();
			//ebo.Bind();

			shader.SetUniform("camPos", Camera.main.transform.position);

			shader.SetUniform("model", Matrix4.Identity);
			shader.SetUniform("view", camera.ViewMatrix);
			shader.SetUniform("projection", camera.ProjectionMatrix);
			shader.SetUniform("nearClip", camera.NearClip);
			shader.SetUniform("farClip", camera.FarClip);

			GL.DrawElementsInstanced(PrimitiveType.Triangles, this.ebo.indexLength, DrawElementsType.UnsignedInt, 0, numInstances);

			ErrorCode errorCode = GL.GetError();

			if (errorCode != ErrorCode.NoError)
			{
				Log.GL($"Error: {errorCode}");
			}
			//base.Render();
		}
	}
}