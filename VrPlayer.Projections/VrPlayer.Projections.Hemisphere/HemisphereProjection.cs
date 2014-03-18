using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using VrPlayer.Contracts.Projections;

namespace VrPlayer.Projections.Hemisphere
{
    [DataContract]
    public class HemisphereProjection : ProjectionBase, IProjection
    {
        private Point3D _center;
        private double _radius = 1;
        private const double Distance = 1000;
        private const double HorizontalCoverage = 0.5D;

        public static readonly DependencyProperty SlicesProperty =
            DependencyProperty.Register("Slices", typeof(int),
            typeof(HemisphereProjection), new FrameworkPropertyMetadata(16));
        [DataMember]
        public int Slices
        {
            get { return (int)GetValue(SlicesProperty); }
            set { SetValue(SlicesProperty, value); }
        }

        public static readonly DependencyProperty StacksProperty =
             DependencyProperty.Register("Stacks", typeof(int),
             typeof(HemisphereProjection), new FrameworkPropertyMetadata(16));
        [DataMember]
        public int Stacks
        {
            get { return (int)GetValue(StacksProperty); }
            set { SetValue(StacksProperty, value); }
        }

        public static readonly DependencyProperty CoverageProperty =
            DependencyProperty.Register("Coverage", typeof(double),
            typeof(HemisphereProjection), new FrameworkPropertyMetadata(0.5D));
        [DataMember]
        public double Coverage
        {
            get { return (double)GetValue(CoverageProperty); }
            set { SetValue(CoverageProperty, value); }
        }

        public static readonly DependencyProperty FishEyeFOVHorizontalProperty =
            DependencyProperty.Register("FishEyeFOVHorizontal", typeof(double),
            typeof(HemisphereProjection), new FrameworkPropertyMetadata(180D));
        [DataMember]
        public double FishEyeFOVHorizontal
        {
            get { return (double)GetValue(FishEyeFOVHorizontalProperty); }
            set { SetValue(FishEyeFOVHorizontalProperty, value); }
        }

        public static readonly DependencyProperty FishEyeFOVVerticalProperty =
            DependencyProperty.Register("FishEyeFOVVertical", typeof(double),
            typeof(HemisphereProjection), new FrameworkPropertyMetadata(180D));
        [DataMember]
        public double FishEyeFOVVertical
        {
            get { return (double)GetValue(FishEyeFOVVerticalProperty); }
            set { SetValue(FishEyeFOVVerticalProperty, value); }
        }

        public static readonly DependencyProperty RollLeftProperty =
            DependencyProperty.Register("RollLeft", typeof(int),
            typeof(HemisphereProjection), new FrameworkPropertyMetadata(0));
        [DataMember]
        public int RollLeft
        {
            get { return (int)GetValue(RollLeftProperty); }
            set { SetValue(RollLeftProperty, value); }
        }

        public static readonly DependencyProperty RollRightProperty =
            DependencyProperty.Register("RollRight", typeof(int),
            typeof(HemisphereProjection), new FrameworkPropertyMetadata(0));
        [DataMember]
        public int RollRight
        {
            get { return (int)GetValue(RollRightProperty); }
            set { SetValue(RollRightProperty, value); }
        }

        public static readonly DependencyProperty YLeftProperty =
            DependencyProperty.Register("YLeft", typeof(int),
            typeof(HemisphereProjection), new FrameworkPropertyMetadata(0));
        [DataMember]
        public int YLeft
        {
            get { return (int)GetValue(YLeftProperty); }
            set { SetValue(YLeftProperty, value); }
        }

        public static readonly DependencyProperty YRightProperty =
            DependencyProperty.Register("YRight", typeof(int),
            typeof(HemisphereProjection), new FrameworkPropertyMetadata(0));
        [DataMember]
        public int YRight
        {
            get { return (int)GetValue(YRightProperty); }
            set { SetValue(YRightProperty, value); }
        }

        public static readonly DependencyProperty XLeftProperty =
            DependencyProperty.Register("XLeft", typeof(int),
            typeof(HemisphereProjection), new FrameworkPropertyMetadata(0));
        [DataMember]
        public int XLeft
        {
            get { return (int)GetValue(XLeftProperty); }
            set { SetValue(XLeftProperty, value); }
        }

        public static readonly DependencyProperty XRightProperty =
            DependencyProperty.Register("XRight", typeof(int),
            typeof(HemisphereProjection), new FrameworkPropertyMetadata(0));
        [DataMember]
        public int XRight
        {
            get { return (int)GetValue(XRightProperty); }
            set { SetValue(XRightProperty, value); }
        }

        public Point3D Center
        {
            get { return _center; }
            set
            {
                _center = value;
                OnPropertyChanged("Center");
            }
        }

        public double Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                OnPropertyChanged("Radius");
            }
        }

        public new Vector3D CameraLeftPosition
        {
            get
            {
                return new Vector3D(Distance + _radius, 0, 0);
            }
        }

        public new Vector3D CameraRightPosition
        {
            get
            {
                return new Vector3D(-Distance - _radius, 0, 0);
            }
        }

        public override Point3DCollection Positions
        {
            get
            {
                var positions = new Point3DCollection();

                //Left
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks * Coverage;
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = HorizontalCoverage * slice * 4 * Math.PI / Slices + Math.PI * (0.5 - HorizontalCoverage);

                        double x = scale * Math.Sin(theta) + Radius;
                        double z = scale * Math.Cos(theta);

                        var normal = new Vector3D(x + Distance, y, z) + Center;

                        var axis = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0 + 90);
                        var trans = new RotateTransform3D(axis, new Point3D(0, 0, 0));
                        var vector = Vector3D.Multiply(new Vector3D(normal.X, normal.Y, normal.Z), trans.Value);
                        positions.Add(new Point3D(vector.X, vector.Y, vector.Z));
                    }
                }

                //Right
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks * Coverage;
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = HorizontalCoverage * slice * 4 * Math.PI / Slices + Math.PI * (0.5 - HorizontalCoverage);

                        double x = scale * Math.Sin(theta) - Radius;
                        double z = scale * Math.Cos(theta);

                        var normal = new Vector3D(x - Distance, y, z) + Center;

                        var axis = new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0 + 90);
                        var trans = new RotateTransform3D(axis, new Point3D(0, 0, 0));
                        var vector = Vector3D.Multiply(new Vector3D(normal.X, normal.Y, normal.Z), trans.Value);
                        positions.Add(new Point3D(vector.X, vector.Y, vector.Z));
                    }
                }

                return positions;
            }
        }

        public override Int32Collection TriangleIndices
        {
            get
            {
                var triangleIndices = new Int32Collection();

                //Left
                for (int stack = 0; stack < Stacks; stack++)
                {
                    int top = (stack + 0) * (Slices + 1);
                    int bot = (stack + 1) * (Slices + 1);

                    for (int slice = 0; slice < Slices; slice++)
                    {
                        if (stack != 0)
                        {
                            triangleIndices.Add(top + slice);
                            triangleIndices.Add(bot + slice);
                            triangleIndices.Add(top + slice + 1);
                        }
                        triangleIndices.Add(top + slice + 1);
                        triangleIndices.Add(bot + slice);
                        triangleIndices.Add(bot + slice + 1);
                    }
                }

                //Right
                for (int stack = Stacks + 1; stack <= (Stacks * 2); stack++)
                {
                    int top = (stack + 0) * (Slices + 1);
                    int bot = (stack + 1) * (Slices + 1);

                    for (int slice = 0; slice < Slices; slice++)
                    {
                        if (stack != Stacks + 1)
                        {
                            triangleIndices.Add(top + slice);
                            triangleIndices.Add(bot + slice);
                            triangleIndices.Add(top + slice + 1);
                        }
                        triangleIndices.Add(top + slice + 1);
                        triangleIndices.Add(bot + slice);
                        triangleIndices.Add(bot + slice + 1);
                    }
                }

                return triangleIndices;
            }
        }

        public override PointCollection MonoTextureCoordinates
        {
            get
            {
                double fisheyeScaleHorizontal = 1.0;
                double fisheyeScaleVertical = 1.0;
                fisheyeScaleHorizontal -= (FishEyeFOVHorizontal - 180.0) / 180.0;
                fisheyeScaleVertical -= (FishEyeFOVVertical - 180.0) / 180.0;
                var textureCoordinates = new PointCollection();

                //Left
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.5 + fisheyeScaleHorizontal * (p.Y / 2) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * RollLeft / 360);
                        var v = 0.5 + fisheyeScaleVertical * (p.Y / 2) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * RollLeft / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //Right
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.5 + fisheyeScaleHorizontal * (p.Y / 2) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * RollRight / 360);
                        var v = 0.5 + fisheyeScaleVertical * (p.Y / 2) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * RollRight / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                return textureCoordinates;
            }
        }

        public override PointCollection OverUnderTextureCoordinates
        {
            get
            {
                double fisheyeScale = 1.0;
                if (FishEyeFOVHorizontal > 180.0) fisheyeScale -= (FishEyeFOVHorizontal - 180.0) / 180.0;
                var textureCoordinates = new PointCollection();

                //Left
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.5 + fisheyeScale * (p.Y / 2) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * RollLeft / 360);
                        var v = 0.25 + fisheyeScale * (p.Y / 4) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * RollLeft / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //Right
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.5 + fisheyeScale * (p.Y / 2) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * RollRight / 360);
                        var v = 0.75 + fisheyeScale * (p.Y / 4) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * RollRight / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                return textureCoordinates;
            }

        }

        public override PointCollection SideBySideTextureCoordinates
        {
            get
            {
                double fisheyeScaleHorizontal = 1.0;
                double fisheyeScaleVertical = 1.0;
                fisheyeScaleHorizontal -= (FishEyeFOVHorizontal - 180.0) / 180.0;
                fisheyeScaleVertical -= (FishEyeFOVVertical - 180.0) / 180.0;
                var textureCoordinates = new PointCollection();

                //Left
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = XLeft / 180.0 + 0.25 + fisheyeScaleHorizontal * (p.Y / 4) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * RollLeft / 360);
                        var v = YLeft / 180.0 + 0.5 + fisheyeScaleVertical * (p.Y / 2) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * RollLeft / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //Right
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = XRight / 180.0 + 0.75 + fisheyeScaleHorizontal * (p.Y / 4) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * RollRight / 360);
                        var v = YRight / 180.0 + 0.5 + fisheyeScaleVertical * (p.Y / 2) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * RollRight / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                return textureCoordinates;
            }
        }

        private double DegToRad(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
