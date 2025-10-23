import 'dart:convert';
import 'package:http/http.dart' as http;
import '../config/api_config.dart';
import 'token_storage.dart';

class AuthService {
  final _store = TokenStorage();

  Future<Map<String, dynamic>> login(String username, String password) async {
    final url = Uri.parse('${ApiConfig.auth}/login');
    final res = await http.post(
      url,
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({'username': username, 'password': password}),
    );
    final data = jsonDecode(res.body.isEmpty ? '{}' : res.body);
    if (res.statusCode == 200) {
      await _store.save(data['access_token']);
      return data;
    }
    throw Exception(
      data['message'] ?? 'Đăng nhập thất bại (${res.statusCode})',
    );
  }

  Future<Map<String, dynamic>> register(
    String username,
    String password,
    int roleId,
  ) async {
    final token = await _store.read();
    final url = Uri.parse('${ApiConfig.auth}/register');
    final res = await http.post(
      url,
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer $token',
      },
      body: jsonEncode({
        'username': username,
        'password': password,
        'roleId': roleId,
      }),
    );
    final data = jsonDecode(res.body.isEmpty ? '{}' : res.body);
    if (res.statusCode == 200) return data;
    throw Exception(data['message'] ?? 'Đăng ký thất bại (${res.statusCode})');
  }

  Future<void> changePassword(String oldPass, String newPass) async {
    final token = await _store.read();
    final url = Uri.parse('${ApiConfig.auth}/change-password');
    final res = await http.post(
      url,
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer $token',
      },
      body: jsonEncode({'oldPassword': oldPass, 'newPassword': newPass}),
    );
    if (res.statusCode != 200) {
      final data = jsonDecode(res.body.isEmpty ? '{}' : res.body);
      throw Exception(
        data['message'] ?? 'Đổi mật khẩu thất bại (${res.statusCode})',
      );
    }
  }

  Future<void> adminResetPassword(int userId, String newPass) async {
    final token = await _store.read();
    final url = Uri.parse('${ApiConfig.auth}/users/$userId/reset-password');
    final res = await http.post(
      url,
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer $token',
      },
      body: jsonEncode({'newPassword': newPass}),
    );
    if (res.statusCode != 200) {
      final data = jsonDecode(res.body.isEmpty ? '{}' : res.body);
      throw Exception(
        data['message'] ?? 'Reset mật khẩu thất bại (${res.statusCode})',
      );
    }
  }

  Future<void> logout() => TokenStorage().clear();

  Future<List<Map<String, dynamic>>> roleOptions() async {
    final token = await _store.read();
    final url = Uri.parse('${ApiConfig.users}/role-options');
    final res = await http.get(
      url,
      headers: {'Authorization': 'Bearer $token'},
    );
    if (res.statusCode == 200) {
      final list = jsonDecode(res.body) as List;
      return list.cast<Map<String, dynamic>>();
    }
    throw Exception('Không tải được role-options (${res.statusCode})');
  }
}
