import { NativeStackScreenProps } from '@react-navigation/native-stack';
import { RootStackParamList } from '../../navigation/MainNavigator';
import { Layout } from '../Basic/Layout/Layout';
import { View, Text, StyleSheet } from 'react-native';
import { globalStyle } from '../../constants/GlobalStyles';

type Props = NativeStackScreenProps<RootStackParamList, 'GameOverPage'>;

export const GameOverPage = ({ navigation, route }: Props) => {
  return (
    <>
      <Layout onBacckBtn={() => navigation.navigate('StartPage')}>
        <View style={style.titleContainer}>
          <Text style={[globalStyle.titleText, style.title]}>Game Over</Text>
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
