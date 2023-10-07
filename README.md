# Read Only Attribute

Utility in the form of an attribute to make public fields greyed out in the editor.

## Usage
```C#
[ReadOnly] public float unEditablePublicFloat = 5;
[ReadOnly, SerializeField] private float unEditablePrivateFloat = 5;
```