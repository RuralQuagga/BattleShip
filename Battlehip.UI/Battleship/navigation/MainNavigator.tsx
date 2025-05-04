import { createNativeStackNavigator } from '@react-navigation/native-stack';
import StartPage from '../components/StartPage/StartPage';
import StatisticPage from '../components/StartPage/StatisticPage/StatisticPage';
import { PlayPage } from '../components/StartPage/PlayPage/PlayPage';

export type RootStackParamList = {
  StartPage: undefined;
  StatisticPage: undefined;
  PlayPage: undefined;
};

const Stack = createNativeStackNavigator<RootStackParamList>();

const MainNavigator = () => {
  return (
    <Stack.Navigator initialRouteName='StartPage'>
      <Stack.Screen name='StartPage' component={StartPage} options={ {headerShown: false} } />
      <Stack.Screen name='StatisticPage' component={StatisticPage} options={{headerShown: false}} />
      <Stack.Screen name='PlayPage' component={PlayPage} options={{headerShown: false}} />
    </Stack.Navigator>
  );
};

export default MainNavigator