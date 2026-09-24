namespace Software9119.Collection.Superb.TestArrangement.TestAide;

sealed class DoubleValueItem ( int primary, int secondary )
{
  public int Primary { get; set; } = primary;
  public int Secondary { get; set; } = secondary;

  static public implicit operator DoubleValueItem ( (int, int) pair ) => new ( pair.Item1, pair.Item2 );
  static public implicit operator DoubleValueItem ( int primary ) => new ( primary, primary );

  public bool ValuesEqual () => Primary == Secondary;
  static public bool ValuesEqual (DoubleValueItem item) => item.Primary == item.Secondary;
}
