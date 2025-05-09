import { useEffect, useState } from 'react';
import { CellType, CheckCellApiRequest, FieldDto } from '../../models/field/fieldMatrix';
import { View, StyleSheet, Pressable } from 'react-native';
import { CheckCell } from '../../endpoints/batleFieldEndpoints';

export type Props = {
  field: FieldDto | undefined,
  isReadOnly: boolean
};

export function Field(props: Props) {
  const {...otherProps } = props;
  const [field, setField] = useState<FieldDto>();

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
      case CellType.ForbiddenMiss || CellType.Miss :
        return 'blue'
      default:
        return 'green'
    }
  }; 
  
  const checkCell = async (line: number, cell: number) =>{
    if(!field){
      throw new Error("Field is undefined");
    }
    const request: CheckCellApiRequest = {
      fieldId: field.fieldId,
      line,
      cell
    };

    const result = await CheckCell(request);
    setField(result.data);
  }

  useEffect(()=>{
    if(!field){
      setField(props.field);
    }    
  })

  const fieldData = field?.fieldConfiguration.map((line, index) => (
    <View key={index} style={style.line}>
      {line.map((cell, cellIndex) => (
        <Pressable style={style.prsbl} onPress={() => {!props.isReadOnly && checkCell(index, cellIndex)}}>        
        <View          
          key={index + cellIndex}          
          style={[{backgroundColor: getColorByType(cell)}, style.cell]}
        />
        </Pressable>
      ))}
    </View>
  ));
  
  return (
    <View style={style.field} {...otherProps}>
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
