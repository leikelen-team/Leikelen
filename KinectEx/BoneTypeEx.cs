﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace KinectEx
{
    /// <summary>
    /// A class that contains values for each "bone" in a Kinect
    /// <c>Body</c>. The class can be used statically in much the
    /// same way as an enum. However, each value contains useful
    /// information about the bone. The class also allows for easy
    /// enumeration of bones.
    /// </summary>
    [Newtonsoft.Json.JsonConverter(typeof(Json.BoneTypeExConverter))]
    public class BoneTypeEx
    {
        /// <summary>
        /// The starting joint of the bone.
        /// </summary>
        public JointTypeEx StartJoint { get; set; }

        /// <summary>
        /// The ending joint of hte bone.
        /// </summary>
        public JointTypeEx EndJoint { get; set; }

        /// <summary>
        /// A short name for the bone (matches the static/enumerated name).
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// A friendlier display name for the bone.
        /// </summary>
        public String DisplayName { get; set; }

        /// <summary>
        /// The bone on the opposite side of the body to this one (if applicable).
        /// </summary>
        public BoneTypeEx MirroredBone
        {
            get
            {
                return BoneTypeEx.MirroredBones[this];
            }
        }

        /// <summary>
        /// A list containing all of the bones in the body, including
        /// "fake" ones like HipFull and ShoulderFull.
        /// </summary>
        public static List<BoneTypeEx> AllBones { get; private set; }

        /// <summary>
        /// A list containing all of the bones in a body that should
        /// be drawn when displaying a skeleton. Effectively all bones
        /// excluding the "fake" ones.
        /// </summary>
        public static List<BoneTypeEx> DrawnBones { get; private set; }

        /// <summary>
        /// A convenient means of mapping a bone to the bone on the
        /// opposite side of the body.
        /// </summary>
        public static Dictionary<BoneTypeEx, BoneTypeEx> MirroredBones { get; private set; }

        /// <summary>
        /// A convenient means of mapping a bone's string name to the
        /// actual BoneTypeEx value.
        /// </summary>
        public static Dictionary<string, BoneTypeEx> ByName { get; private set; }

        // ==============================================================
        // Below the Waist
        // ==============================================================
        /// <summary>
        /// Gets the hip left.
        /// </summary>
        public static BoneTypeEx HipLeft { get; private set; }

        /// <summary>
        /// Gets the hip right.
        /// </summary>
        public static BoneTypeEx HipRight { get; private set; }

        /// <summary>
        /// Gets the leg upper left.
        /// </summary>
        public static BoneTypeEx LegUpperLeft { get; private set; }

        /// <summary>
        /// Gets the leg upper right.
        /// </summary>
        public static BoneTypeEx LegUpperRight { get; private set; }

        /// <summary>
        /// Gets the leg lower left.
        /// </summary>
        public static BoneTypeEx LegLowerLeft { get; private set; }

        /// <summary>
        /// Gets the leg lower right.
        /// </summary>
        public static BoneTypeEx LegLowerRight { get; private set; }

        /// <summary>
        /// Gets the foot left.
        /// </summary>
        public static BoneTypeEx FootLeft { get; private set; }

        /// <summary>
        /// Gets the foot right.
        /// </summary>
        public static BoneTypeEx FootRight { get; private set; }

        // ==============================================================
        // Above the Waist
        // ==============================================================

        /// <summary>
        /// Gets the spine lower.
        /// </summary>
        public static BoneTypeEx SpineLower { get; private set; }

        /// <summary>
        /// Gets the spine upper.
        /// </summary>
        public static BoneTypeEx SpineUpper { get; private set; }

        /// <summary>
        /// Gets the shoulder left.
        /// </summary>
        public static BoneTypeEx ShoulderLeft { get; private set; }

        /// <summary>
        /// Gets the shoulder right.
        /// </summary>
        public static BoneTypeEx ShoulderRight { get; private set; }

        /// <summary>
        /// Gets the arm upper left.
        /// </summary>
        public static BoneTypeEx ArmUpperLeft { get; private set; }

        /// <summary>
        /// Gets the arm upper right.
        /// </summary>
        public static BoneTypeEx ArmUpperRight { get; private set; }

        /// <summary>
        /// Gets the arm lower left.
        /// </summary>
        public static BoneTypeEx ArmLowerLeft { get; private set; }

        /// <summary>
        /// Gets the arm lower right.
        /// </summary>
        public static BoneTypeEx ArmLowerRight { get; private set; }

        /// <summary>
        /// Gets the hand left.
        /// </summary>
        public static BoneTypeEx HandLeft { get; private set; }

        /// <summary>
        /// Gets the hand right.
        /// </summary>
        public static BoneTypeEx HandRight { get; private set; }

        /// <summary>
        /// Gets the head.
        /// </summary>
        public static BoneTypeEx Head { get; private set; }

        // ==============================================================
        // "Fake" Bones
        // ==============================================================

        /// <summary>
        /// Gets the hip full.
        /// </summary>
        public static BoneTypeEx HipFull { get; private set; }

        /// <summary>
        /// Gets the shoulder full.
        /// </summary>
        public static BoneTypeEx ShoulderFull { get; private set; }

        /// <summary>
        /// Initializes the <see cref="BoneTypeEx"/> class.
        /// </summary>
        static BoneTypeEx()
        {
            HipLeft = new BoneTypeEx() { Name = "HipLeft", DisplayName = "Left Hip", StartJoint = JointTypeEx.SpineBase, EndJoint = JointTypeEx.HipLeft };
            HipRight = new BoneTypeEx() { Name = "HipRight", DisplayName = "Right Hip", StartJoint = JointTypeEx.SpineBase, EndJoint = JointTypeEx.HipRight };
            LegUpperLeft = new BoneTypeEx() { Name = "LegUpperLeft", DisplayName = "Upper Left Leg", StartJoint = JointTypeEx.HipLeft, EndJoint = JointTypeEx.KneeLeft };
            LegUpperRight = new BoneTypeEx() { Name = "LegUpperRight", DisplayName = "Upper Right Leg", StartJoint = JointTypeEx.HipRight, EndJoint = JointTypeEx.KneeRight };
            LegLowerLeft = new BoneTypeEx() { Name = "LegLowerLeft", DisplayName = "Lower Left Leg", StartJoint = JointTypeEx.KneeLeft, EndJoint = JointTypeEx.AnkleLeft };
            LegLowerRight = new BoneTypeEx() { Name = "LegLowerRight", DisplayName = "Lower Right Leg", StartJoint = JointTypeEx.KneeRight, EndJoint = JointTypeEx.AnkleRight };
            FootLeft = new BoneTypeEx() { Name = "FootLeft", DisplayName = "Left Foot", StartJoint = JointTypeEx.AnkleLeft, EndJoint = JointTypeEx.FootLeft };
            FootRight = new BoneTypeEx() { Name = "FootRight", DisplayName = "Right Foot", StartJoint = JointTypeEx.AnkleRight, EndJoint = JointTypeEx.FootRight };
            SpineLower = new BoneTypeEx() { Name = "SpineLower", DisplayName = "Lower Spine", StartJoint = JointTypeEx.SpineBase, EndJoint = JointTypeEx.SpineMid };
            SpineUpper = new BoneTypeEx() { Name = "SpineUpper", DisplayName = "Upper Spine", StartJoint = JointTypeEx.SpineMid, EndJoint = JointTypeEx.SpineShoulder };
            ShoulderLeft = new BoneTypeEx() { Name = "ShoulderLeft", DisplayName = "Left Shoulder", StartJoint = JointTypeEx.SpineShoulder, EndJoint = JointTypeEx.ShoulderLeft };
            ShoulderRight = new BoneTypeEx() { Name = "ShoulderRight", DisplayName = "Right Shoulder", StartJoint = JointTypeEx.SpineShoulder, EndJoint = JointTypeEx.ShoulderRight };
            ArmUpperLeft = new BoneTypeEx() { Name = "ArmUpperLeft", DisplayName = "Upper Left Arm", StartJoint = JointTypeEx.ShoulderLeft, EndJoint = JointTypeEx.ElbowLeft };
            ArmUpperRight = new BoneTypeEx() { Name = "ArmUpperRight", DisplayName = "Upper Right Arm", StartJoint = JointTypeEx.ShoulderRight, EndJoint = JointTypeEx.ElbowRight };
            ArmLowerLeft = new BoneTypeEx() { Name = "ArmLowerLeft", DisplayName = "Lower Left Arm", StartJoint = JointTypeEx.ElbowLeft, EndJoint = JointTypeEx.WristLeft };
            ArmLowerRight = new BoneTypeEx() { Name = "ArmLowerRight", DisplayName = "Lower Right Arm", StartJoint = JointTypeEx.ElbowRight, EndJoint = JointTypeEx.WristRight };
            HandLeft = new BoneTypeEx() { Name = "HandLeft", DisplayName = "Left Hand", StartJoint = JointTypeEx.WristLeft, EndJoint = JointTypeEx.HandLeft };
            HandRight = new BoneTypeEx() { Name = "HandRight", DisplayName = "Right Hand", StartJoint = JointTypeEx.WristRight, EndJoint = JointTypeEx.HandRight };
            Head = new BoneTypeEx() { Name = "Head", DisplayName = "Head", StartJoint = JointTypeEx.SpineShoulder, EndJoint = JointTypeEx.Head };
            HipFull = new BoneTypeEx() { Name = "HipFull", DisplayName = "Full Hip", StartJoint = JointTypeEx.HipLeft, EndJoint = JointTypeEx.HipRight };
            ShoulderFull = new BoneTypeEx() { Name = "ShoulderFull", DisplayName = "Full Shoulders", StartJoint = JointTypeEx.ShoulderLeft, EndJoint = JointTypeEx.ShoulderRight };

            DrawnBones = new List<BoneTypeEx>()
            {
                HipLeft,
                HipRight,
                LegUpperLeft,
                LegUpperRight,
                LegLowerLeft,
                LegLowerRight,
                FootLeft,
                FootRight,
                SpineLower,
                SpineUpper,
                ShoulderLeft,
                ShoulderRight,
                ArmUpperLeft,
                ArmUpperRight,
                ArmLowerLeft,
                ArmLowerRight,
                HandLeft,
                HandRight,
                Head
            };

            AllBones = DrawnBones.ToList();
            AllBones.Add(HipFull);
            AllBones.Add(ShoulderFull);

            MirroredBones = new Dictionary<BoneTypeEx, BoneTypeEx>();
            MirroredBones.Add(HipLeft, HipRight);
            MirroredBones.Add(HipRight, HipLeft);
            MirroredBones.Add(LegUpperLeft, LegUpperRight);
            MirroredBones.Add(LegUpperRight, LegUpperLeft);
            MirroredBones.Add(LegLowerLeft, LegLowerRight);
            MirroredBones.Add(LegLowerRight, LegLowerLeft);
            MirroredBones.Add(FootLeft, FootRight);
            MirroredBones.Add(FootRight, FootLeft);
            MirroredBones.Add(SpineLower, SpineLower);
            MirroredBones.Add(SpineUpper, SpineUpper);
            MirroredBones.Add(ShoulderLeft, ShoulderRight);
            MirroredBones.Add(ShoulderRight, ShoulderLeft);
            MirroredBones.Add(ArmUpperLeft, ArmUpperRight);
            MirroredBones.Add(ArmUpperRight, ArmUpperLeft);
            MirroredBones.Add(ArmLowerLeft, ArmLowerRight);
            MirroredBones.Add(ArmLowerRight, ArmLowerLeft);
            MirroredBones.Add(HandLeft, HandRight);
            MirroredBones.Add(HandRight, HandLeft);
            MirroredBones.Add(Head, Head);
            MirroredBones.Add(HipFull, HipFull);
            MirroredBones.Add(ShoulderFull, ShoulderFull);

            ByName = new Dictionary<string, BoneTypeEx>();
            foreach (var bone in AllBones)
            {
                ByName.Add(bone.Name, bone);
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var bone = obj as BoneTypeEx;
            return bone != null && bone.Name == this.Name;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="BoneTypeEx"/>.
        /// </summary>
        /// <param name="boneName">Name of the bone.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator BoneTypeEx(string boneName)
        {
            return ByName[boneName];
        }
    }
}
