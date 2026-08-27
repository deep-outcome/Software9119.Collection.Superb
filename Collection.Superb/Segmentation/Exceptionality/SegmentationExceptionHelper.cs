using System.Diagnostics;

namespace Software9119.Collection.Superb.Segmentation;

class SegmentationExceptionHelper
{
  static public string DebugValMsg ( string? msg )
  {
    Debug.Assert ( msg != null );
    return msg;
  }
}
