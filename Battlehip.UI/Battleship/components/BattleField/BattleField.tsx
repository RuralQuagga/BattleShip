import { FieldDto } from '../../models/field/fieldMatrix';
import { Field } from '../Field/Field';
import { NativeStackNavigationProp } from '@react-navigation/native-stack';
import { RootStackParamList } from '../../navigation/MainNavigator';

type Props = {  
  gameField?: FieldDto;
  isReadonly: boolean;
};

export function Battlefield(props: Props) {  
  return (
    <>
      <Field field={props.gameField} isReadOnly={props.isReadonly} />      
    </>
  );
}
