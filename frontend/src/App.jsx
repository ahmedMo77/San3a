import { lazy, Suspense } from "react";
import Footer from "./components/footer";
import Navbar from "./components/navbar";
const Home = lazy(() => import("./pages/Home/Home.jsx"));
import { Provider } from "./components/ui/provider";
import { Routes, Route } from "react-router-dom";
import Login from "./pages/auth/login";
import Signup from "./pages/auth/signup";
import RegisterClientForm from "./components/auth/forms/signup/RegisterClientForm.jsx";
import RegisterWorkerForm from "./components/auth/forms/signup/RegisterWorkerForm.jsx";
import VerifyOtp from "./components/auth/verifyOtp/index.jsx";
import SuccessLogin from "./components/auth/sucess/successLogin.jsx";
import { Toaster } from "./components/ui/toaster.jsx";

function App() {
  return (
    <Provider>
      <Toaster />
      <Suspense>
        <Navbar />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/تسجيل-الدخول" element={<Login />} />
          <Route path="/إنشاء-حساب" element={<Signup />} />
          <Route path="/إنشاء-حساب/عميل" element={<RegisterClientForm />} />
          <Route path="/إنشاء-حساب/حرفي" element={<RegisterWorkerForm />} />
          <Route path="/إنشاء-حساب/تأكيد-الرمز-السري" element={<VerifyOtp />} />
          <Route path="/إنشاء-حساب/نجاح" element={<SuccessLogin />} />
        </Routes>
        <Footer />
      </Suspense>
    </Provider>
  );
}

export default App;
