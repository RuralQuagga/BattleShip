import { CellType, FieldMatrix } from '../../models/field/fieldMatrix';
import { ThemeProps } from '../Themed';
import { View as DefaultView, StyleSheet } from 'react-native';

export type FieldProps = ThemeProps & DefaultView['props'] & FieldMatrix;

export function Field(props: FieldProps) {
  const { lightColor, darkColor, ...otherProps } = props;

  const getColorByType = (item: CellType): string => {
    switch(item){
      case CellType.Empty: 
        return 'gray';
      case CellType.Forbidden:
        return 'gray';
      case CellType.Ship:
        return 'black';
      default:
        return 'green'
    }
  };

  const fieldData = props.Items.map((line, index) => (
    <DefaultView key={index} style={style.line}>
      {line.map((cell, cellIndex) => (
        <DefaultView
          key={index + cellIndex}          
          style={[{backgroundColor: getColorByType(cell)}, style.cell]}
        />
      ))}
    </DefaultView>
  ));
  
  return (
    <DefaultView style={style.field} {...otherProps}>
      {fieldData}
    </DefaultView>
  );
}

const style = StyleSheet.create({
  field: {
    backgroundColor: '#8aa7db',
    margin: '1%',
    height: '49%',
    width: '96%',
  },
  cell: {    
    borderColor: 'black',
    borderRadius: '10%',
    borderWidth: 1.5,
    flex: 1,
    opacity: 0.5,
    padding: 1
  },
  line: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    flex: 1
  }
});
