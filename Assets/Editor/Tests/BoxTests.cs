using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[TestFixture]
public class BoxTests
{
    [Test]
    /// <summary>
    /// Tests that on creation, a <see cref="Box"/> has exactly one of each type of <see cref="Face.Side"/>
    /// </summary>
    public void BoxTest0_BoxHasCorrectFaces()
    {
        Box box = new Box();
        List<Face.Side> faces = box.faces.Select(f => f.side).ToList();
        Assert.AreEqual(6, faces.Count());
        Assert.Contains(Face.Side.Front, faces);
        Assert.Contains(Face.Side.Back, faces);
        Assert.Contains(Face.Side.Top, faces);
        Assert.Contains(Face.Side.Bottom, faces);
        Assert.Contains(Face.Side.Left, faces);
        Assert.Contains(Face.Side.Right, faces);
    }

    [Test]
    /// <summary>
    /// Tests that on creation, a <see cref="Box"/>'s <see cref="Face.Side"/>s have the right rotation
    /// </summary>
    public void BoxTest1_BoxFacesAreCorrectlyOriented()
    {
        Box box = new Box();
        foreach (Face face in box.faces)
        {
            if (face.side == Face.Side.Back)
            {
                Assert.AreEqual(180, face.rotation);
            }
            else
            {
                Assert.AreEqual(0, face.rotation);
            }
        }
    }

    [Test]
    /// <summary>
    /// Tests that a <see cref="Stamp"/> on an unrotated <see cref="Box"/> is applied to the front <see cref="Face"/>
    /// </summary>
    public void BoxTest2_Stamp()
    {
        Box box = new Box();
        box.Stamp();
        Face face = box.faces.Where(f => f.stamps.Count > 0).First();
        Assert.AreEqual(Face.Side.Front, face.side);
    }

    [Test]
    /// <summary>
    /// Tests that a <see cref="Stamp"/> is corrctly applied to the <see cref="Box"/> after rotating left around the Y axis
    /// </summary>
    public void BoxTest3_LeftRotateAroundY()
    {
        Box box = new Box();
        box.RotateYLeft();
        box.Stamp();
        Face face = box.faces.Where(f => f.stamps.Count > 0).First();
        Assert.AreEqual(Face.Side.Right, face.side);
        Assert.AreEqual(0, face.stamps[0].rotation);
    }

    [Test]
    /// <summary>
    /// Tests that a <see cref="Stamp"/> is corrctly applied to the <see cref="Box"/> after rotating counterclockwise facing the forward direction
    /// </summary>
    public void BoxTest4_LeftRotateAroundZ()
    {
        Box box = new Box();
        box.RotateZLeft();
        box.Stamp();
        Face face = box.faces.Where(f => f.stamps.Count > 0).First();
        Assert.AreEqual(Face.Side.Front, face.side);
        Assert.AreEqual(270, face.stamps[0].rotation);
    }

    [Test]
    /// <summary>
    /// Tests that a <see cref="Stamp"/> is corrctly applied to the <see cref="Box"/> after a series of rotations
    /// </summary>
    public void BoxTest5_MultipleRotations()
    {
        Box box = new Box();
        box.RotateYLeft();
        box.RotateZLeft();
        box.RotateYLeft();
        box.Stamp();
        Face face = box.faces.Where(f => f.stamps.Count > 0).First();
        Assert.AreEqual(Face.Side.Bottom, face.side);
        Assert.AreEqual(180, face.stamps[0].rotation);
    }
}
