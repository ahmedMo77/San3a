import Feedback from "./components/feedback"
import Footer from "./components/footer"
import Hero from "./components/hero"
import Navbar from "./components/navbar"
import OurServices from "./components/ourservices"
import OurWork from "./components/ourwork"
import OurWorkers from "./components/ourworkers"
import ReadySection from "./components/readysection"
import { Provider } from "./components/ui/provider"
import WhyUs from "./components/whyus"
import { Divider } from "./sharedComponents/Divider"

function App() {

  return (
    <Provider>
      <Navbar />
      <Hero />
      <Divider />
      <OurServices />
      <Divider />
      <WhyUs/>
      <Divider />
      <OurWork/>
      <Divider />
      <OurWorkers/>
      <Divider />
      <Feedback/>
      <Divider />
      <ReadySection/>
      <Footer/>
    </Provider>
  )
}

export default App
