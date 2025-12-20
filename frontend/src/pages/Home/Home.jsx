import Hero from "../../components/hero/index";
import { Divider } from "../../sharedComponents/Divider";
import OurServices from "../../components/ourservices";
import WhyUs from "../../components/whyus";
import OurWork from "../../components/ourwork";
import OurWorkers from "../../components/ourworkers";
import Feedback from "../../components/feedback";
import ReadySection from "../../components/readysection";

function Home() {
  return (
    <>
      <Hero />
      <Divider />
      <OurServices />
      <Divider />
      <WhyUs />
      <Divider />
      <OurWork />
      <Divider />
      <OurWorkers />
      <Divider />
      <Feedback />
      <Divider />
      <ReadySection />
    </>
  );
}

export default Home;
