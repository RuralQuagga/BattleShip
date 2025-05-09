import { NativeStackScreenProps } from "@react-navigation/native-stack";
import { RootStackParamList } from "../../navigation/MainNavigator";
import { Layout } from "../Basic/Layout/Layout";
import { View, Text, StyleSheet } from "react-native";
import { Battlefield } from "../BattleField/BattleField";
import { globalStyle } from "../../constants/GlobalStyles";

type Props = NativeStackScreenProps<RootStackParamList, 'GamePage'>;

export const GamePage = ({navigation, route}: Props) =>{
    const { sessionId, field } = route.params;
    return(<>
    <Layout>
        <View style={style.titleContainer}>
                  <Text style={[globalStyle.titleText, style.title]}>
                    Game
                  </Text>
                </View>
                <View style={style.gameFieldContainer}>
                  <Battlefield gameField={field} isReadonly={false}/>
                </View>
    </Layout>
    </>)
}

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