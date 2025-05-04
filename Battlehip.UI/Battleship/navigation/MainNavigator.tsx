import { createNativeStackNavigator } from '@react-navigation/native-stack';
import StartPage from '../components/StartPage/StartPage';
import Records from '../components/StartPage/Records/Records';

export type RootStackParamList = {
  StartPage: undefined;
  Records: { id: number };
};

const Stack = createNativeStackNavigator<RootStackParamList>();

const MainNavigator = () => {
  return (
    <Stack.Navigator initialRouteName='StartPage'>
      <Stack.Screen name='StartPage' component={StartPage} options={ {headerShown: false} } />
      <Stack.Screen name='Records' component={Records} />
    </Stack.Navigator>
  );
};

export default MainNavigator