import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from '../../../navigation/MainNavigator';
import { Layout } from '../../Basic/Layout/Layout';
import { Battlefield } from '../../BattleField/BattleField';
import { StyleSheet, View, Text } from 'react-native';
import { globalStyle } from '../../../constants/GlobalStyles';

type Props = NativeStackScreenProps<RootStackParamList, 'PlayPage'>;

export const PlayPage = ({ navigation }: Props) => {
  return (
    <>
      <Layout onBacckBtn={() => navigation.navigate('StartPage')}>
        <View style={style.titleContainer}>
            <Text style={[globalStyle.titleText, style.title]}>Prepare to game</Text>
        </View>
        <View style={style.gameFieldContainer}>
          <Battlefield />
        </View>
      </Layout>
    </>
  );
};

const style = StyleSheet.create({
    titleContainer:{
        flex: 1
    },
    title:{
        marginTop: '10%'
    },
    gameFieldContainer:{
        flex: 5
    }
})