using System;
using System.Text;

namespace hashes;

public class GhostsTask : IFactory<Cat>, IFactory<Document>,
    IFactory<Robot>, IFactory<Segment>, IFactory<Vector>, IMagic
{
    private FactoryType type;
    private Cat cat;
    private byte[] docBytes;
    private Robot robot;
    private Segment segment;
    private Vector vector;

    public void DoMagic()
    {
        switch (type)
        {
            case FactoryType.Cat:
                cat.Rename("dog");
                break;
            case FactoryType.Document:
                docBytes[0] = 1;
                break;
            case FactoryType.Robot:
                Robot.BatteryCapacity = 20;
                break;
            case FactoryType.Segment:
                segment.Start.Add(new Vector(20, 20));
                break;
            case FactoryType.Vector:
                vector.Add(new Vector(20, 20));
                break;
            default:
                throw new ArgumentException();
        }
    }

    Cat IFactory<Cat>.Create()
    {
        if (cat is null) cat = new Cat("cat", "black", DateTime.MaxValue);
        type = FactoryType.Cat;
        return cat;
    }

    Document IFactory<Document>.Create()
    {
        if (docBytes is null) docBytes = new byte[] { 0, 0, 0 };
        type = FactoryType.Document;
        return new Document("doc", Encoding.Default, docBytes);
    }

    Robot IFactory<Robot>.Create()
    {
        if (robot is null) robot = new Robot("rob");
        type = FactoryType.Robot;
        return robot;
    }

    Segment IFactory<Segment>.Create()
    {
        if (segment is null) segment = new Segment(new Vector(0, 0), new Vector(0, 0));
        type = FactoryType.Segment;
        return segment;
    }

    Vector IFactory<Vector>.Create()
    {
        if (vector is null) vector = new Vector(0, 0);
        type = FactoryType.Vector;
        return vector;
    }

    enum FactoryType { Cat, Document, Robot, Segment, Vector }
}
