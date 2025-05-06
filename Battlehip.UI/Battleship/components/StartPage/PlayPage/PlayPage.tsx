import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from '../../../navigation/MainNavigator';
import { Layout } from '../../Basic/Layout/Layout';
import { Battlefield } from '../../BattleField/BattleField';
import { StyleSheet, View, Text, ActivityIndicator } from 'react-native';
import { globalStyle } from '../../../constants/GlobalStyles';
import { useEffect, useState } from 'react';
import { startSession } from '../../../endpoints/gameSessionEndpoints';
import { addNew } from '../../../store/reducers/gameSessionReducer';

type Props = NativeStackScreenProps<RootStackParamList, 'PlayPage'>;

export const PlayPage = ({ navigation }: Props) => {
    const [sessionId, setSessionId] = useState("");
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string>('');

    const fetchData = async () =>{
        try {
              const response = await startSession();
              setSessionId(response.data as string);
              console.log(response.data as string);
              addNew(response.data as string);
            } catch (err) {
              setError((err as Error).message);
            } finally {
              setLoading(false);
            }
    }

  useEffect(()=>{ 
    fetchData();  
  },[])  

  if (loading) {
      return <ActivityIndicator size='large' />;
    }
  
    if (error !== '') {
      return <Text>Ошибка: {error}</Text>;
    }
    return (
    <>
      <Layout onBacckBtn={() => navigation.navigate('StartPage')}>
        <View style={style.titleContainer}>
          <Text style={[globalStyle.titleText, style.title]}>
            Prepare to game
          </Text>
        </View>
        <View style={style.gameFieldContainer}>
          <Battlefield sessionId={sessionId} />
        </View>
      </Layout>
    </>
  );
};

const style = StyleSheet.create({
  titleContainer: {
    flex: 1,
  },
  title: {
    marginTop: '10%',
  },
  gameFieldContainer: {
    flex: 5,
  },
});
