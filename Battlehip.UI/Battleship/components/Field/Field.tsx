import { CellType, FieldDto } from '../../models/field/fieldMatrix';
import { View, StyleSheet, Pressable } from 'react-native';

export type Props = {
  field: FieldDto | undefined,
  isReadOnly: boolean,
  checkCell: (index:number, cellIndex: number) => void,
};

export function Field(props: Props) {
  const getColorByType = (item: CellType): string => {
    switch(item){
      case CellType.Empty: 
        return 'gray';
      case CellType.Forbidden:
        return 'gray';
      case CellType.Ship:
        return 'black';
      case CellType.DeadShip:
        return 'red';
      case CellType.ForbiddenMiss:
        return 'blue';
      case CellType.Miss:
        return 'blue';
      default:
        return 'green'
    }
  };    

  const fieldData = props.field?.fieldConfiguration.map((line, index) => (
    <View key={index} style={style.line}>
      {line.map((cell, cellIndex) => (
        <Pressable style={style.prsbl} onPress={() => {!props.isReadOnly && props.checkCell(index, cellIndex)}}>        
        <View          
          key={index + cellIndex}          
          style={[{backgroundColor: getColorByType(cell)}, style.cell]}
        />
        </Pressable>
      ))}
    </View>
  ));
  
  return (
    <View style={style.field}>
      {fieldData}
    </View>
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
  },
  prsbl:{
    flex: 1
  },
  line: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    flex: 1
  }
});
