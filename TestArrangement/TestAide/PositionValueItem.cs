namespace Software9119.Collection.Superb.TestArrangement.TestAide;

struct PositionValueItem ( int value, int position )
{
  public int Position { readonly get => initialized ? position : -1; set => position = value; }
  public int Value { readonly get => initialized ? value : -1; set => this.value = value; }

  readonly bool initialized = true;
  int position = position;
  int value = value;

  static public implicit operator PositionValueItem ( (int, int) pair ) => new ( pair.Item1, pair.Item2 );
}
