import { Text } from 'react-native';
import {
  ActivityIndicator,
  View as DefaultView,
  StyleSheet,
} from 'react-native';
import { useEffect, useState } from 'react';
import { CellType, FieldDto } from '../../models/field/fieldMatrix';
import { GetBatlefield } from '../../endpoints/batleFieldEndpoints';
import { Field } from '../Field/Field';

type Props = {
  sessionId: string
}

export function Battlefield(props: Props) {  
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>('');
  const [myField, setMyField] = useState<FieldDto>();
  const [apponentField, setApponentField] = useState<CellType[][]>([[]]);

  const fetchData = async () => {
    try {
      const response = await GetBatlefield(props.sessionId, true);
      setMyField(response.data as FieldDto);
    } catch (err) {
      setError((err as Error).message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  if (loading) {
    return <ActivityIndicator size='large' />;
  }

  if (error !== '') {
    return <Text>Ошибка: {error}</Text>;
  }

  return (
    <>
      <Field matrix={myField?.fieldConfiguration} />
      {/* <Field {...otherProps} Items={apponentField} /> */}
    </>
  );
}

const style = StyleSheet.create({
  field: {
    backgroundColor: '#8aa7db',
    margin: '1%',
    height: '49%',
    width: '96%',
  },
  fieldName: {
    fontSize: 15,
    fontWeight: '600',
    opacity: 0.5,
    color: 'black',
  },
});
