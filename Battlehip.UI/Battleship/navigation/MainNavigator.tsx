import { createNativeStackNavigator } from '@react-navigation/native-stack';
import StartPage from '../components/StartPage/StartPage';
import StatisticPage from '../components/StartPage/StatisticPage/StatisticPage';
import { PlayPage } from '../components/StartPage/PlayPage/PlayPage';
import { GamePage } from '../components/GamePage/GamePage';
import { ActionState, FieldDto } from '../models/field/fieldMatrix';
import { UserField } from '../components/UserField/UserField';
import { GameOverPage } from '../components/GameOverPage/GameOverPage';

export type RootStackParamList = {
  StartPage: undefined;
  StatisticPage: undefined;
  PlayPage: undefined;
  GamePage: { field?: FieldDto; sessionId: string | null };
  UserField: { sessionId: string };
  GameOverPage: { state: ActionState };
};

const Stack = createNativeStackNavigator<RootStackParamList>();

const MainNavigator = () => {
  return (
    <Stack.Navigator initialRouteName='StartPage'>
      <Stack.Screen
        name='StartPage'
        component={StartPage}
        options={{ headerShown: false }}
      />
      <Stack.Screen
        name='StatisticPage'
        component={StatisticPage}
        options={{ headerShown: false }}
      />
      <Stack.Screen
        name='PlayPage'
        component={PlayPage}
        options={{ headerShown: false }}
      />
      <Stack.Screen
        name='GamePage'
        component={GamePage}
        options={{ headerShown: false }}
      />
      <Stack.Screen
        name='UserField'
        component={UserField}
        options={{ headerShown: false }}
      />
      <Stack.Screen
        name='GameOverPage'
        component={GameOverPage}
        options={{ headerShown: false }}
      />
    </Stack.Navigator>
  );
};

export default MainNavigator;
