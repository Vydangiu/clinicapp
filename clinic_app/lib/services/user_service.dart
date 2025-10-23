import 'dart:convert';
import 'package:http/http.dart' as http;
import '../config/api_config.dart';
import 'token_storage.dart';

class UserService {
  final _store = TokenStorage();

  Future<Map<String, String>> _header() async {
    final t = await _store.read();
    return {
      'Content-Type': 'application/json',
      if (t != null) 'Authorization': 'Bearer $t',
    };
  }

  Future<List<Map<String, dynamic>>> list() async {
    final res = await http.get(
      Uri.parse(ApiConfig.users),
      headers: await _header(),
    );
    if (res.statusCode == 200) {
      final list = jsonDecode(res.body) as List;
      return list.cast<Map<String, dynamic>>();
    }
    throw Exception('Không tải được danh sách user (${res.statusCode})');
  }

  Future<void> toggle(int id) async {
    final res = await http.put(
      Uri.parse('${ApiConfig.users}/$id/toggle'),
      headers: await _header(),
    );
    if (res.statusCode != 200) {
      throw Exception('Không thể khóa/mở user (${res.statusCode})');
    }
  }

  Future<void> delete(int id) async {
    final res = await http.delete(
      Uri.parse('${ApiConfig.users}/$id'),
      headers: await _header(),
    );
    if (res.statusCode != 200) {
      throw Exception('Xoá user thất bại (${res.statusCode})');
    }
  }

  Future<void> updateRole(int id, int roleId) async {
    final res = await http.put(
      Uri.parse('${ApiConfig.users}/$id/role'),
      headers: await _header(),
      body: jsonEncode({'roleId': roleId}),
    );
    if (res.statusCode != 200) {
      throw Exception('Cập nhật quyền thất bại (${res.statusCode})');
    }
  }
}
