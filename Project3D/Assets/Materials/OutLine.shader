Shader "Custom/Outline"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_Outline("Outline", Float) = 0.1
		_OutlineColor("Outline Color", Color) = (1,1,1,1)
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			//Queue : Queue태그를 사용하여 오브젝트를 드로우 처리할 순서를 결정
			//Transparet : Geometry와 AlphaTest 후 뒤에서 앞순서로 렌더링
			//RenderType : 셰이더를 몇가지 사전 정의 그룹으로 카테고리화


			// 외곽선 그리기
			Pass
			{
				Blend SrcAlpha OneMinusSrcAlpha
				Cull Front // 뒷면만 그리기
				ZWrite Off //불투명한 오브젝트에 대한 비활성화 및 반투명 오브젝트 활성화

				CGPROGRAM

				#pragma vertex vert
			//버텍스를 위한 함수 vert를 만들겠다는 뜻
			//Vertex : 각 물체의 점
				#pragma fragment frag
			//fragment를 위한 함수 frag를 만들겠다는 뜻
			//Fragment : 각각의 픽셀이 어떤 색상으로 그려질지 정의

				half _Outline;
				half4 _OutlineColor;

				struct vertexInput
				{
					float4 vertex: POSITION;
				};

				struct vertexOutput
				{
					float4 pos: SV_POSITION;
				};

				float4 CreateOutline(float4 vertPos, float Outline)
				{
					// 행렬 중에 크기를 조절하는 부분만 값을 넣는다.
					// 밑의 부가 설명 사진 참고.
					float4x4 scaleMat;
					scaleMat[0][0] = 1.0f + Outline; //원래 크기에 OutLine수치를 더해준다.
					scaleMat[0][1] = 0.0f;
					scaleMat[0][2] = 0.0f;
					scaleMat[0][3] = 0.0f;
					scaleMat[1][0] = 0.0f;
					scaleMat[1][1] = 1.0f + Outline; //원래 크기에 OutLine수치를 더해준다.
					scaleMat[1][2] = 0.0f;
					scaleMat[1][3] = 0.0f;
					scaleMat[2][0] = 0.0f;
					scaleMat[2][1] = 0.0f;
					scaleMat[2][2] = 1.0f + Outline; //원래 크기에 OutLine수치를 더해준다.
					scaleMat[2][3] = 0.0f;
					scaleMat[3][0] = 0.0f;
					scaleMat[3][1] = 0.0f;
					scaleMat[3][2] = 0.0f;
					scaleMat[3][3] = 1.0f;

					return mul(scaleMat, vertPos);
				}

				vertexOutput vert(vertexInput v)
				{
					vertexOutput o;

					o.pos = UnityObjectToClipPos(CreateOutline(v.vertex, _Outline));
					//UnityObjectToClipPos : Vertex의 좌표를 받아서 투영좌표로 변환
						return o;
					}

					half4 frag(vertexOutput i) : COLOR
					{
						return _OutlineColor;
					}

					ENDCG
				}

			// 정상적으로 그리기
			Pass
			{
				Blend SrcAlpha OneMinusSrcAlpha

				CGPROGRAM

				#pragma vertex vert //버텍스 셰이더 선언
				#pragma fragment frag //프래그먼트 셰이더 선언

				half4 _Color;
				sampler2D _MainTex;
				float4 _MainTex_ST;

				struct vertexInput
				{
					float4 vertex: POSITION;
					float4 texcoord: TEXCOORD0;
				};

				struct vertexOutput
				{
					float4 pos: SV_POSITION;
					float4 texcoord: TEXCOORD0;
				};

				vertexOutput vert(vertexInput v)
				{
					vertexOutput o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.texcoord.xy = (v.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw);
					return o;
				}

				half4 frag(vertexOutput i) : COLOR
				{
					return tex2D(_MainTex, i.texcoord) * _Color;
				}

				ENDCG
			}
		}
}

// * Shader -> Unlit 으로 생성

// * 외곽선 그리기
//- 원래 오브젝트의 행렬을 곱해 크기를 키워준다. (외곽선이 된다.)
//- 두번째 원래 오브젝트를 그려준다.

// * 행렬
//- 오브젝트 Vertex에 행렬을 곱해주면 행렬 위치에 따라 위치, 회전, 크기를 조절할 수 있다.

// * 이동행렬
//[ 1 0 0 x ]
//[ 0 1 0 y ] 
//[ 0 0 1 z ]
//[ 0 0 0 1 ]
//- x축으로 10만큼 이동시키고자 한다면 x자리에 10을 곱해주면 된다.

// * 회전행렬
//- 축마다 회전행렬이 다르다. (생략)

// * 크기행렬
//[ x 0 0 0 ]
//[ 0 y 0 0 ]
//[ 0 0 z 0 ]
//[ 0 0 0 1 ]
//- x, y, z는 스케일값이 된다.

